using MultiplatformBot.Models;

namespace MultiplatformBot.Commands
{
    public sealed class Test : Command
    {
        public Test(VkConversation vkConversation, string arguments)
        {
            VkConversation = vkConversation;
            Arguments = arguments;
        }

        public override VkConversation VkConversation { get; set; }
        public override string Arguments { get; set; }
        public override void Execute()
        {
            
            
            throw new System.NotImplementedException("Test Exception");
        }
    }
}