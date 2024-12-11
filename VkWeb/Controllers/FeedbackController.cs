using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using VkCore.Dtos;

namespace VkWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IEmailSender _email;

        public FeedbackController(
            IEmailSender email
        )
        {
            _email = email;
        }

        [HttpPost]
        public async Task<IActionResult> Post(FeedbackDto dto, CancellationToken cancellationToken)
        {
            var message = $"{dto.FullName}<br /><br />{dto.Email}<br /><br />{dto.Message}";
            var subject = $"VK Feedback: {(string.IsNullOrWhiteSpace(dto.Email) ? DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : dto.Email)}";
            await _email.SendEmailAsync("contact@vocabkitchen.com", subject, message);
            return Ok();
        }
    }
}