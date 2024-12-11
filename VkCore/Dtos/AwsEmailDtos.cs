using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VkCore.Dtos
{
    /// <summary>Represents the bounce or complaint notification stored in Amazon SQS.</summary>
    public class AmazonSqsNotification
    {
        public string Type { get; set; }
        public string Message { get; set; }
    }

    /// <summary>Represents an Amazon SES bounce notification.</summary>
    public class AmazonSesBounceNotification
    {
        public string NotificationType { get; set; }
        public AmazonSesBounce Bounce { get; set; }

        public bool IsPermanentBounce()
        {
            return this.NotificationType == "Bounce" && this.Bounce.BounceType == "Permanent";
        }

        public IEnumerable<string> GetEmails()
        {
            return this.Bounce.BouncedRecipients.Select(r => r.EmailAddress);
        }
    }
    /// <summary>Represents meta data for the bounce notification from Amazon SES.</summary>
    public class AmazonSesBounce
    {
        public string BounceType { get; set; }
        public string BounceSubType { get; set; }
        public DateTime Timestamp { get; set; }
        public List<AmazonSesBouncedRecipient> BouncedRecipients { get; set; }
    }
    /// <summary>Represents the email address of recipients that bounced
    /// when sending from Amazon SES.</summary>
    public class AmazonSesBouncedRecipient
    {
        public string EmailAddress { get; set; }
    }

    /// <summary>Represents an Amazon SES complaint notification.</summary>
    public class AmazonSesComplaintNotification
    {
        public string NotificationType { get; set; }
        public AmazonSesComplaint Complaint { get; set; }
    }
    /// <summary>Represents the email address of individual recipients that complained 
    /// to Amazon SES.</summary>
    public class AmazonSesComplainedRecipient
    {
        public string EmailAddress { get; set; }
    }
    /// <summary>Represents meta data for the complaint notification from Amazon SES.</summary>
    public class AmazonSesComplaint
    {
        public List<AmazonSesComplainedRecipient> ComplainedRecipients { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageId { get; set; }
    }
}
