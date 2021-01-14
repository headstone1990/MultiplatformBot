using System.Collections.Generic;

namespace MultiplatformBot.Models
{
    public abstract class Conversation
    {
        public abstract int Id{ get; protected init; }
    }
}