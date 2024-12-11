using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkCore.Interfaces;

namespace VkInfrastructure.Email
{
    public class AwsEmailSender : IEmailSender
    {
        private readonly IEmailConfig _emailConfig;

        public AwsEmailSender(IEmailConfig emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = _emailConfig.VkContactAddress,
                    Destination = new Destination
                    {
                        ToAddresses =
                            new List<string> { email }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = htmlMessage
                            }
                        }
                    },
                    // If you are not using a configuration set, comment
                    // or remove the following line 
                    //ConfigurationSetName = configSet
                };

                try
                {
                    var response = await client.SendEmailAsync(sendRequest);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine("Error message: " + ex.Message);

                }
            }
        }
    }
}
