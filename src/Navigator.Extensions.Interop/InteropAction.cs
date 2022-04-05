using Microsoft.Extensions.Logging;
using Navigator.Actions;
using Navigator.Actions.Attributes;
using Navigator.Context;
using Navigator.Context.Extensions.Bundled.OriginalEvent;
using Python.Runtime;

namespace Navigator.Extensions.Interop;

[ActionPriority(Actions.Priority.High)]
public class InteropAction : ProviderAgnosticAction
{
    public readonly (string Path, string Name) Module;
    
    public InteropAction(INavigatorContextAccessor navigatorContextAccessor, (string Path, string Name) module) : base(navigatorContextAccessor)
    {
        Module = module;
    }

    public override bool CanHandleCurrentContext()
    {
        using (Py.GIL()) {
            dynamic sys = Py.Import("sys");
            sys.path.append(Module.Path);
            dynamic action = Py.Import(Module.Name);

            try
            {
                return action.can_handle_current_context(NavigatorContextAccessor.NavigatorContext.GetOriginalEvent());
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}