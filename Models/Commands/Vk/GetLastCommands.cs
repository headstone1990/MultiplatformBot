namespace MultiplatformBot.Models.Commands.Vk
{
    public sealed class GetLastCommands : VkCommand
    {
        public GetLastCommands(string arguments)
        {
            Arguments = arguments;
        }

        public override string Arguments { get; set; }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}