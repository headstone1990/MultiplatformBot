using System.Collections.Generic;
using MultiplatformBot.Models.Commands.Vk;
using VkNet.Model;

namespace MultiplatformBot.Models
{
    public sealed class VkConversation : Conversation
    {
        
        public VkConversation(long vkId)
        {
            VkId = vkId;
            messages = new LinkedList<VkNet.Model.Message>();
            commands = new LinkedList<VkNet.Model.Message>();
        }

        public override int Id { get; protected init; }

        public long VkId { get; private init; }

        private LinkedList<VkNet.Model.Message> messages;
        private LinkedList<VkNet.Model.Message> commands;



        public void AddMessage(VkNet.Model.Message message)
        {
            messages.AddFirst(message);
            if (messages.Count > 100)
            {
                messages.RemoveLast();
            }
        }
        
        public void AddCommand(VkNet.Model.Message command)
        {
            ParseCommand(command);
                commands.AddFirst(command);
            if (commands.Count > 100)
            {
                commands.RemoveLast();
            }
        }

        private void ParseCommand(VkNet.Model.Message command)
        {
            if (command.Text == "!test")
            {
                var commandInstance = new Test(command.Text);
                commandInstance.Execute();
            }
            else if(command.Text.Contains("GetLastCommands"))
            {
                
            }
            else
            {
                AddMessage(command);
            }
        }
        
    }
}