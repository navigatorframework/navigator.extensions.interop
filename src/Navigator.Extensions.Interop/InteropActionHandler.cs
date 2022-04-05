using Navigator.Actions;
using Navigator.Context;
using Navigator.Context.Extensions.Bundled.OriginalEvent;
using Python.Runtime;

namespace Navigator.Extensions.Interop;

public abstract class InteropActionHandler : ActionHandler<InteropAction>
{
    protected InteropActionHandler(INavigatorContextAccessor navigatorContextAccessor) : base(navigatorContextAccessor)
    {
    }

    public override async Task<Status> Handle(InteropAction action, CancellationToken cancellationToken)
    {
        using (Py.GIL()) {
            dynamic sys = Py.Import("sys");
            sys.path.append(action.Module.Path);
            dynamic handler = Py.Import(action.Module.Name);

            try
            {
                handler.handle(NavigatorContext.Conversation, NavigatorContext.GetOriginalEvent(), await ArgsBuilder());

                return Success();
            }
            catch (Exception e)
            {
                return new Status(false);
            }
        }
    }

    protected abstract Task<object> ArgsBuilder();
}