using VkBot.Models;

namespace VkBot.Commands
{
    public sealed class Test : Command
    {
        public Test(Chat chat, string arguments)
        {
            Chat = chat;
            Arguments = arguments;
        }

        public override Chat Chat { get; set; }
        public override string Arguments { get; set; }
        public override void Execute()
        {
            
            
            throw new System.NotImplementedException("Test Exception");
        }
    }
}