namespace MultiplatformBot.Models.Commands
{
    public abstract class Command
    {
        public abstract string Arguments { get; set; }
        
        
        public abstract void Execute();
    }
}