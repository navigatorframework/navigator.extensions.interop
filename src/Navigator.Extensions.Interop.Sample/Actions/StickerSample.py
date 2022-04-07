from Telegram.Bot import TelegramBotClientExtensions as navigator


def can_handle_current_context(context, event):
    return (event.Message is not None) and (event.Message.Text is None)


def handle(conversation, event, args):
    navigator.SendTextMessageAsync(args.client, args.chat, "Hello :3")
