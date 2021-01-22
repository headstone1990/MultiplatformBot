namespace MultiplatformBot.Models.Commands
{
    public abstract class Command
    {
        protected Command(string[]? arguments)
        {
            Arguments = arguments;
        }

        protected string[]? Arguments { get; }
        
        
        public abstract void Execute();
    }
}