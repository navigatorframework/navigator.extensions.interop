using Microsoft.Extensions.Logging;
using Navigator.Actions;
using Navigator.Context;
using Navigator.Context.Extensions.Bundled.OriginalEvent;
using Python.Runtime;

namespace Navigator.Extensions.Interop;

public abstract class InteropActionHandler : ActionHandler<InteropAction>
{
    private readonly ILogger<InteropActionHandler> _logger;

    /// <inheritdoc />
    protected InteropActionHandler(INavigatorContextAccessor navigatorContextAccessor, ILogger<InteropActionHandler> logger) : base(navigatorContextAccessor)
    {
        _logger = logger;
    }

    /// <inheritdoc />
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
                _logger.LogError(e, "Unhandled error inside interop action handler for {@Action}", action);
                return new Status(false);
            }
        }
    }

    protected abstract Task<object> ArgsBuilder();
}