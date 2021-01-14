namespace MultiplatformBot.Models
{
    public class VkMessage : Message
    {
        public override int Id { get; set; }
        public long VkId { get; }
        public long ConversationId { get; }
        
        public long FromId { get; }
        public override string? Text { get; set; }

        public VkMessage(long vkId, long conversationId, long fromId)
        {
            VkId = vkId;
            ConversationId = conversationId;
            FromId = fromId;
        }
    }
}