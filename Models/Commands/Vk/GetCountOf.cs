using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace MultiplatformBot.Models.Commands.Vk
{
    public class GetCountOf : VkCommand
    {
        public GetCountOf(string[]? argument, VkConversation vkConversation) : base(argument, vkConversation)
        {
        }

        public override void Execute()
        {
            if (Arguments == null)
            {
                throw new NullReferenceException();
            }

            if (Arguments.Length > 1)
            {
                throw new NotImplementedException("Пока не поддерживается больше 1 аргумента");
            }

            if (Arguments[0].Length > 1)
            {
                throw new NotImplementedException("Пока поддерживаются только отдельные символы");
            }

            var targetSymbol = Arguments[0][0];


            ulong? lastMessageId = null;
            if (VkConversation.Messages.First != null)
            {
                lastMessageId = (ulong) VkConversation.Messages.First.ValueRef.ConversationId;
            }

            if (lastMessageId == null)
            {
                throw new NullReferenceException();
            }

            List<ulong> messagesIDsList = new();
            if (messagesIDsList == null) throw new ArgumentNullException(nameof(messagesIDsList));
            for (var i = lastMessageId.Value; i > 0; i--)
            {
                messagesIDsList.Add(i);
            }

            var chunks = messagesIDsList.Batch(100);

            var sw = new Stopwatch();
            sw.Start();
            List<VkNet.Model.Message> vkMessages = GetVkMessages(chunks);
            sw.Stop();

            int count = 0;

            foreach (var message in vkMessages)
            {
                if (string.IsNullOrEmpty(message.Text)) continue;
                foreach (var symbol in message.Text)
                {
                    if (symbol == targetSymbol) count++;
                }
            }


            VkConversation.VkApi.Messages.Send(new MessagesSendParams()
            {
                RandomId = new DateTime().Millisecond,
                PeerId = VkConversation.VkId,
                Message = $"The '{targetSymbol}' was used {count} times"
            });


            Console.WriteLine(sw.Elapsed);

            VkConversation.VkApi.Messages.Send(new MessagesSendParams()
            {
                RandomId = new DateTime().Millisecond,
                PeerId = VkConversation.VkId,
                Message = $"Всего сообщений: {vkMessages.Count} На запрос потрачено: {sw.Elapsed}"
            });
        }

        private List<VkNet.Model.Message> GetVkMessages(IEnumerable<IEnumerable<ulong>> chunks)
        {
            var vkMessages = new List<VkNet.Model.Message>();
            var tasks = new List<Task<GetByConversationMessageIdResult>>();
            var vkApi = VkConversation.VkApi;


            foreach (var chunk in chunks)
            {
                var vkMessagesChunk = vkApi.Messages.GetByConversationMessageId(VkConversation.VkId, chunk, null).Items
                    .ToList();

                if (!vkMessagesChunk.Any()) break;
                
                vkMessages.AddRange(vkMessagesChunk);
            }


            return vkMessages;
        }
    }
}