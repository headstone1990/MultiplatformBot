namespace MultiplatformBot.Models
{
    public abstract class Message
    {
        public abstract int Id { get; set; }
        public abstract string? Text { get; set; }
    }
}