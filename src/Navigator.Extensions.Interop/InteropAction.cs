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
    private readonly (string Path, string Name) _module;
    private 
    public InteropAction(INavigatorContextAccessor navigatorContextAccessor, (string Path, string Name) module) : base(navigatorContextAccessor)
    {
        _module = module;
    }

    public override bool CanHandleCurrentContext()
    {
        using (Py.GIL()) {
            dynamic sys = Py.Import("sys");
            sys.path.append(_module.Path);
            dynamic action = Py.Import(_module.Name);

            try
            {
                return action.can_handle_current_context(this.NavigatorContextAccessor.NavigatorContext.GetOriginalEvent()));
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}