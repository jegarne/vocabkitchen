﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VkCore.Models.TagModel;

namespace VkCore.Interfaces
{
    public interface ITagService
    {
        Task<Tag> GetDefaultTagAsync(string orgId);
    }
}
