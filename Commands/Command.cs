using VkBot;

namespace VKBotOld.Commands
{
    public abstract class Command
    {
        public abstract Chat Chat { get; set; }
        public abstract string Arguments { get; set; }
        
        
        public abstract void Execute();
    }
}