namespace MultiplatformBot.Models
{
    public class VkMessage : Message
    {
        public override int Id { get; set; }
        public long ConversationId { get; }
        
        public long FromId { get; }
        public override string? Text { get; set; }

        public VkMessage(long conversationId, long fromId)
        {
            ConversationId = conversationId;
            FromId = fromId;
        }
    }
}