using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;
using Chat = VkBot.Models.Chat;

namespace VkBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IVkApi vkApi;


        public CallbackController(IVkApi vkApi, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.vkApi = vkApi;
        }

        private List<Chat> chats = new();

        [HttpPost]
        public IActionResult Callback([FromBody] Updates updates)
        {
            switch (updates.Type)
            {
                case "confirmation":
                {
                    return Ok(configuration["Config:Confirmation"]);
                }

                case "message_new":
                {
                    var response = new VkResponse(updates.Object);
                    var message = Message.FromJson(response);
                    var clientInfo = ClientInfo.FromJson(response);

                    try
                    {
                       if (chats.All(chat => chat.Id != message.PeerId))
                       {
                           chats.Add(new Chat(message.PeerId.GetValueOrDefault()));
                       }

                       ParseMessage(message);
                    }
                    catch (Exception exception)
                    {
                        vkApi.Messages.Send(new MessagesSendParams()
                        {
                            RandomId = new DateTime().Millisecond,
                            PeerId = message.PeerId.GetValueOrDefault(),
                            Message = exception.ToString()
                        });
                    }
                    break;
                }
            }

            return Ok("ok");
        }

        private void ParseMessage(Message message)
        {
            var text = message.Text.Trim();
            var chat = chats.FirstOrDefault(e => e.Id == message.PeerId);
            if (text[0] == '!')
            {
                chat?.AddCommand(message);
            }
            else
            {
                chat?.AddMessage(message);
            }
        }

        private void SendResponse(Message message, string text)
        {
            vkApi.Messages.Send(new MessagesSendParams()
            {
                RandomId = new DateTime().Millisecond,
                PeerId = message.PeerId.GetValueOrDefault(),
                Message = text
            });
        }
    }
}