using System.Collections.Generic;
using VkBot.Commands;
using VkNet.Model;

namespace VkBot.Models
{
    public class Chat
    {
        public Chat(long id)
        {
            Id = id;
            messages = new LinkedList<Message>();
            commands = new LinkedList<Message>();
        }

        public long Id { get; }

        private LinkedList<Message> messages;
        private LinkedList<Message> commands;

        private Conversation conversation;

        public void AddMessage(Message message)
        {
            messages.AddFirst(message);
            if (messages.Count > 100)
            {
                messages.RemoveLast();
            }
        }
        
        public void AddCommand(Message command)
        {
            ParseCommand(command);
                commands.AddFirst(command);
            if (commands.Count > 100)
            {
                commands.RemoveLast();
            }
        }

        private void ParseCommand(Message command)
        {
            if (command.Text == "!test")
            {
                var commandInstance = new Test(this, command.Text);
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