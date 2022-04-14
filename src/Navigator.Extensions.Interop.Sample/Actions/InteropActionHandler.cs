using Navigator.Context;
using Navigator.Providers.Telegram;
using Telegram.Bot.Types;

namespace Navigator.Extensions.Interop.Sample.Actions;

public class CustomInteropActionHandler : InteropActionHandler
{
    public CustomInteropActionHandler(INavigatorContextAccessor navigatorContextAccessor, ILogger<CustomInteropActionHandler> logger) : base(navigatorContextAccessor, logger)
    {
    }

    protected override Task<object> ArgsBuilder()
    {
        return Task.FromResult<object>(new
        {
            client = NavigatorContext.GetTelegramClient(),
            chat = new ChatId(NavigatorContext.GetTelegramChat().Id)
        });
    }
}