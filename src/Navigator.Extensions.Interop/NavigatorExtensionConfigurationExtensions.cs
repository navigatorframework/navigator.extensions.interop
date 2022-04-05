using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Navigator.Actions;
using Navigator.Configuration;
using Navigator.Configuration.Extension;
using Navigator.Context;
using Python.Runtime;

namespace Navigator.Extensions.Interop;

public static class NavigatorExtensionConfigurationExtensions
{
    public static NavigatorConfiguration Interop(this NavigatorExtensionConfiguration configuration)
    {
        try
        {
            Runtime.PythonDLL = "libpython3.so";
            PythonEngine.Initialize();
            PythonEngine.BeginAllowThreads();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return configuration.Extension(configuration =>
        {
            configuration.Services.LoadAllInteropActions();

            var a = typeof(InteropAction).IsAssignableTo(typeof(IAction));

            var actions = configuration.Services
                .Where(descriptor => descriptor.ImplementationType?.IsAssignableTo(typeof(IAction)) 
                                     ?? descriptor.ImplementationFactory?.Method.ReturnType.IsAssignableTo(typeof(IAction))
                                     ?? false)
                .Select(descriptor => descriptor.ImplementationType ?? descriptor.ImplementationFactory!.Method.ReturnType)
                .ToArray();
            
            configuration.Options.RegisterActionsCore(actions, true);
        
            configuration.Options.RegisterActionsPriorityCore(actions, true);
        });
    }

    private static void LoadAllInteropActions(this IServiceCollection services)
    {
        foreach(var modulePath in Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.py", SearchOption.AllDirectories))
        {
            var path = Path.GetDirectoryName(modulePath);
            var name = Path.GetFileNameWithoutExtension(modulePath);

            if (path is not null)
            {
                services.AddScoped<InteropAction, InteropAction>(provider =>
                {
                    var accessor = provider.GetRequiredService<INavigatorContextAccessor>();

                    return new InteropAction(accessor, (path, name));
                });
            }
        }
    }
}