using Navigator.Actions;
using Navigator.Context;
using Navigator.Context.Extensions.Bundled.OriginalEvent;
using Navigator.Providers.Telegram;
using Python.Runtime;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Navigator.Extensions.Interop.Sample.Actions;

public class EchoActionHandler : ActionHandler<EchoAction>
{
    public EchoActionHandler(INavigatorContextAccessor navigatorContextAccessor) : base(navigatorContextAccessor)
    {
    }

    public override async Task<Status> Handle(EchoAction action, CancellationToken cancellationToken)
    {
        // try
        // {
        //     using (Py.GIL()) {
        //         dynamic os = Py.Import("os");
        //         dynamic sys = Py.Import("sys");
        //         sys.path.append(os.getcwd());
        //         Console.WriteLine("----");
        //         Console.WriteLine(os.getcwd());
        //         Console.WriteLine("=");
        //         Console.WriteLine(Directory.GetCurrentDirectory());
        //         Console.WriteLine("----");
        //         dynamic a = Py.Import("Actions.InteropAction");
        // Console.WriteLine(a.func(NavigatorContext.GetOriginalEvent<Update>()));      
        //     }
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e);
        //     throw;
        // }
        // var test = Directory.GetCurrentDirectory();
        //
        await this.GetTelegramClient().SendTextMessageAsync(this.GetTelegramChat().Id, 
            action.MessageToEcho, cancellationToken: cancellationToken);

        return Success();
    }
}