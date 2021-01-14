using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MultiplatformBot.Models;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace MultiplatformBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VkCallbackController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IVkApi vkApi;
        private readonly ApplicationContext db;


        public VkCallbackController(IVkApi vkApi, IConfiguration configuration, ApplicationContext db)
        {
            this.configuration = configuration;
            this.db = db;
            this.vkApi = vkApi;
        }

        //private List<VkConversation> chats = new();

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
                    var message = VkNet.Model.Message.FromJson(response);
                    var clientInfo = ClientInfo.FromJson(response);
                    if (message.PeerId == null)
                    {
                        throw new NullReferenceException("Message PeerId is null");
                    }

                    var peerId = message.PeerId.Value;
                    VkConversation conversation = null!;

                    foreach (var dbConversation in db.VkConversations)
                    {
                        if (dbConversation.VkId != peerId) continue;
                        conversation = dbConversation;
                        break;
                    }
                    conversation ??= new VkConversation(peerId);

                    // try
                    // {
                    //    if (chats.All(chat => chat.Id != message.PeerId))
                    //    {
                    //        chats.Add(new VkConversation(message.PeerId.GetValueOrDefault()));
                    //    }
                    //
                    //    ParseMessage(message);
                    // }
                    // catch (Exception exception)
                    // {
                    //     vkApi.Messages.Send(new MessagesSendParams()
                    //     {
                    //         RandomId = new DateTime().Millisecond,
                    //         PeerId = message.PeerId.GetValueOrDefault(),
                    //         Message = exception.ToString()
                    //     });
                    // }
                    break;
                }
            }

            return Ok("ok");
        }

        // private bool IsNewConversation(long? messagePeerId)
        // {
        //     foreach (var conversation in db.Conversations)
        //     {
        //         if (conversation is not VkConversation vkConversation) continue;
        //         if (vkConversation.VkId == messagePeerId)
        //         {
        //             return false;
        //         }
        //     }
        //
        //     return true;
        // }

        // private void ParseMessage(VkNet.Model.Message message)
        // {
        //     var text = message.Text.Trim();
        //     var chat = chats.FirstOrDefault(e => e.Id == message.PeerId);
        //     if (string.IsNullOrEmpty(text))
        //     {
        //         return;
        //     }
        //     if (text[0] == '!')
        //     {
        //         chat?.AddCommand(message);
        //     }
        //     else
        //     {
        //         chat?.AddMessage(message);
        //     }
        // }

        private void SendResponse(VkNet.Model.Message message, string text)
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