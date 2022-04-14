using Microsoft.Extensions.Logging;
using Navigator.Actions;
using Navigator.Actions.Attributes;
using Navigator.Context;
using Navigator.Context.Extensions.Bundled.OriginalEvent;
using Python.Runtime;

namespace Navigator.Extensions.Interop;

/// <inheritdoc />
[ActionPriority(2500)]
public sealed class InteropAction : ProviderAgnosticAction
{
    private readonly ILogger<InteropAction> _logger;

    /// <summary>
    /// Python module to load.
    /// </summary>
    public readonly (string Path, string Name) Module;

    /// <inheritdoc />
    public InteropAction(INavigatorContextAccessor navigatorContextAccessor, ILogger<InteropAction> logger, (string Path, string Name) module) : base(navigatorContextAccessor)
    {
        Module = module;
        _logger = logger;
    }

    /// <inheritdoc />
    public override bool CanHandleCurrentContext()
    {
        using (Py.GIL()) {
            dynamic sys = Py.Import("sys");
            sys.path.append(Module.Path);
            dynamic action = Py.Import(Module.Name);

            try
            {
                return action.can_handle_current_context(NavigatorContextAccessor.NavigatorContext, NavigatorContextAccessor.NavigatorContext.GetOriginalEvent());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled error inside action for {@Module}", Module);
                return false;
            }
        }
    }
}