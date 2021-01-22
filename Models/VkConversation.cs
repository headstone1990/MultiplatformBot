using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MultiplatformBot.Models.Commands.Vk;
using VkNet.Abstractions;

namespace MultiplatformBot.Models
{
    public sealed class VkConversation : Conversation
    {
        public IVkApi VkApi { get; }


        public VkConversation(long vkId, IVkApi vkApi)
        {
            VkId = vkId;
            Messages = new LinkedList<VkMessage>();
            this.VkApi = vkApi;
        }
        
        // ReSharper disable UnusedMember.Local
        private VkConversation(DbContext context) // Used by EF
            // ReSharper restore UnusedMember.Local
        {
            VkApi = context.Database.GetService<IVkApi>();
            Messages = new LinkedList<VkMessage>();
        }
        
        
        public override int Id { get; protected init; } //init Used by EF

        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
        public long VkId { get; private init; } //init Used by EF
        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Local


        [NotMapped]
        public LinkedList<VkMessage> Messages { get; }


        public void ParseMessage(VkNet.Model.Message apiMessage)
        {
            if (apiMessage.ConversationMessageId == null || apiMessage.FromId == null) return;

            var conversationMessageId = apiMessage.ConversationMessageId.Value;
            var fromId = apiMessage.FromId.Value;

            Messages.AddFirst(new VkMessage(conversationMessageId, fromId));

            if (Messages.Count > 100) Messages.RemoveLast();

            if (string.IsNullOrEmpty(apiMessage.Text)) return;


            string trimmedText = apiMessage.Text.Trim();
            if (trimmedText[0] == '!')
            {
                var command = VkCommand.ParseCommand(trimmedText, this);
                if (command == null) return;
                command.Execute();
            }
        }
    }
}