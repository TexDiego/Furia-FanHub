using Furia_FanHub.MVVM.Models;

namespace Furia_FanHub.MVVM.Views
{
    public class ChatMessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UserMessageTemplate { get; set; }
        public DataTemplate BotMessageTemplate { get; set; }
        public DataTemplate SystemMessageTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var message = (ChatMessage)item;
            if (message.IsSystemMessage)
                return SystemMessageTemplate;
            return message.IsUser ? UserMessageTemplate : BotMessageTemplate;
        }
    }
}
