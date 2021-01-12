using MultiplatformBot.Models;

namespace MultiplatformBot.Commands
{
    public abstract class Command
    {
        public abstract VkConversation VkConversation { get; set; }
        public abstract string Arguments { get; set; }
        
        
        public abstract void Execute();
    }
}