def can_handle_current_context(update):
    return (update.Message is not None) and (update.Message.Text is not None)


def handle(conversation, args):
    args.SendTextMessageAsync(conversation.Chat.ExternalIdentifier, "HelloFromPython")
