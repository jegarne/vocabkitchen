using System.Threading.Tasks;
using VkCore.Interfaces;
using VkInfrastructure.Email;
using Xunit;

namespace VkInfrastructure.Test.Email
{
    class TestEmailConfig : IEmailConfig
    {
        public string VkContactAddress { get; set; } = "VocabKitchen <jeremy@vocabkitchen.com>";
    }

    public class AwsEmailSenderShould
    {
        [Fact]
        public async Task SendEmail()
        {
            var emailSender = new AwsEmailSender(new TestEmailConfig());

            // let's not blow up my inbox now that we're out of the sandbox
            // await emailSender.SendEmailAsync("success@simulator.amazonses.com", "test", "<b>test</b>");
            // await emailSender.SendEmailAsync("bounce@simulator.amazonses.com", "test", "<b>test</b>");
        }
    }
}
