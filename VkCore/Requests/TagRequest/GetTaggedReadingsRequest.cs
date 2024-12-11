﻿using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.TagRequest
{
    public class GetTaggedReadingsRequest : IRequest<DtoResult<IEnumerable<ReadingDto>>>
    {
        public GetTaggedReadingsRequest(string orgId, string tagId)
        {
            OrgId = orgId;
            TagId = tagId;
        }

        public string OrgId { get; set; }
        public string TagId { get; set; }
    }
}