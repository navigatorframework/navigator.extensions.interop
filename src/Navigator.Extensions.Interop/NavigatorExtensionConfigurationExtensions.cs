using Navigator.Configuration;
using Navigator.Configuration.Extension;
using Navigator.Context.Extensions;
using Python.Included;

namespace Navigator.Extensions.Interop;

public static class NavigatorExtensionConfigurationExtensions
{
    public static NavigatorConfiguration Interop(this NavigatorExtensionConfiguration configuration)
    {
        Installer.SetupPython().GetAwaiter().GetResult();

        return configuration.Extension(
            _ => {},
            services => {});
    }
}