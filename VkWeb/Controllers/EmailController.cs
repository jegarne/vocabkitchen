using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using VkCore.Dtos;
using VkCore.Requests.Email;

namespace VkWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IEmailSender _emailSender;

        public EmailController(IMediator mediator, IEmailSender emailSender)
        {
            _mediator = mediator;
            _emailSender = emailSender;
        }

        [HttpPost]
        [Route("/api/email/bounce")]
        public async Task<IActionResult> PostAsync()
        {
            if (Request.Headers["x-amz-sns-message-type"] == "Notification")
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    try
                    {
                        var jsonData = await reader.ReadToEndAsync();
                        var notification = JsonConvert.DeserializeObject<AmazonSesBounceNotification>(jsonData);
                        await _mediator.Send(new HandleBounceRequest(notification));
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                        return BadRequest();
                    }
                }
            }

            if (Request.Headers["x-amz-sns-message-type"] == "SubscriptionConfirmation")
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    try
                    {
                        var jsonData = await reader.ReadToEndAsync();
                        var snsMessage = Amazon.SimpleNotificationService.Util.Message.ParseMessage(jsonData);

                        //verify the signaure using AWS method
                        if (!snsMessage.IsMessageSignatureValid())
                            throw new Exception("Invalid signature");

                        var subscribeUrl = snsMessage.SubscribeURL;
                        var webClient = new WebClient();
                        webClient.DownloadString(subscribeUrl);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                        return BadRequest();
                    }
                }
            }

            return Ok();
        }
    }
}
