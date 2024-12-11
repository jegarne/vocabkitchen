using VkCore.SharedKernel;

namespace VkCore.Models.TagModel
{
    public class UserTag : BaseEntity
    {
        public UserTag(string userId, string tagId)
        {
            UserId = userId;
            TagId = tagId;
        }
        
        public string TagId { get; set; }
        public Tag Tag { get; set; }

        public string UserId { get; set; }
        public VkUser User { get; set; }
    }
}
