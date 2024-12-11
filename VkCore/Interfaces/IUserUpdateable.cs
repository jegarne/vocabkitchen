using System;

namespace VkCore.Interfaces
{
    public interface IUserUpdateable
    {
        string UpdatedBy { get; set; }
        DateTime UpdateDate { get; set; }
    }
}
