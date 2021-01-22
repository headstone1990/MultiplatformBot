using System;
using System.Linq;
using VkNet;
using VkNet.Model.RequestParams;

namespace MultiplatformBot.Models.Commands.Vk
{
    public sealed class Test : VkCommand
    {
        public Test(string[]? argument, VkConversation vkConversation) : base(argument, vkConversation)
        {
        }


        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}