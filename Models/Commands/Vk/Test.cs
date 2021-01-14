namespace MultiplatformBot.Models.Commands.Vk
{
    public sealed class Test : VkCommand
    {
        public Test(string arguments)
        {
            Arguments = arguments;
        }
        
        public override string Arguments { get; set; }
        public override void Execute()
        {
            
            
            throw new System.NotImplementedException("Test Exception");
        }
    }
}