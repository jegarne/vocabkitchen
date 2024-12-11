using System.Collections.Generic;
using VkCore.Models.Organization;

namespace VkCore.Dtos
{
    public class OrgDto
    {
        public OrgDto(Org org)
        {
            Id = org.Id;
            Name = org.Name;

            if (org.Readings != null)
            {
                Readings = new List<ReadingDto>();
                foreach (var reading in org.Readings)
                {
                    Readings.Add(new ReadingDto(reading.Reading));
                }
            }

            if (org.Students != null)
            {
                Students = new List<OrgStudentDto>();
                foreach (var student in org.Students)
                {
                    Students.Add(new OrgStudentDto(student.StudentUser));
                }
            }

            if (org.Tags != null)
            {
                Tags = new List<TagDto>();
                foreach (var tag in org.Tags)
                {
                    Tags.Add(new TagDto(tag.Id, tag.Value, tag.IsDefault));
                }
            }
        }

        public static IEnumerable<OrgDto> BuildOrgDtos(IEnumerable<Org> orgs)
        {
            foreach (var org in orgs)
            {
                yield return new OrgDto(org);
            }
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public List<ReadingDto> Readings { get; set; }
        public List<OrgStudentDto> Students { get; set; }
        public List<TagDto> Tags { get; set; }
    }
}
