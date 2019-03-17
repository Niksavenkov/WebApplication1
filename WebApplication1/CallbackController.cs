using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using System;

namespace Cookie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]





    public class CallbackController : ControllerBase
    {

        static string[] Commands = { "hello" };

        /// <summary>
        /// Конфигурация приложения
        /// </summary>
        private readonly IConfiguration _configuration;

        private readonly IVkApi _vkApi;

        public CallbackController(IVkApi vkApi, IConfiguration configuration)
        {
            _configuration = configuration;

            _vkApi = vkApi;





            

        }

        



        [HttpPost]
        public IActionResult Callback([FromBody] Updates updates)
        {
            // Проверяем, что находится в поле "type" 
            switch (updates.Type)
            {
                


                // Если это уведомление для подтверждения адреса
                case "confirmation":
                    // Отправляем строку для подтверждения 
                    return Ok(_configuration["Config:Confirmation"]);

                case "message_new":
                    {
                        // Десериализация
                        var msg = Message.FromJson(new VkResponse(Updates.Object));

                        long? userID = msg.UserId;

                        // Отправим в ответ полученный от пользователя текст
                        void SendMessage (string message)
                        {

                            _vkApi.Messages.Send(new MessagesSendParams
                            {


                                RandomId = new DateTime().Millisecond,
                                PeerId = msg.PeerId.Value,
                                Message = message,
                                UserId = userID

                            });
                        }

                        SendMessage("Привет");

                        break;
                    }

                    

            }
            // Возвращаем "ok" серверу Callback API
            return Ok("ok");

            
        }





    }
}
