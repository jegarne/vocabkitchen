using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkCore.Models.ReadingModel;
using VkCore.Models.Word;

namespace VkCore.Dtos
{
    public class ReadingDto
    {
        public ReadingDto(Reading reading)
        {
            Id = reading.Id;
            Title = reading.Title;
            Text = reading.Text;
            ContentItems = new List<ContentItemDto>();
            Tags = new List<TagDto>();
        }

        public string Id { get; set; }
        public string OrgId { get; set; }
        public string Title { get; set; }
        public int NewWords { get; set; }
        public int KnownWords { get; set; }
        public int InProgressWords { get; set; }
        public string Text { get; set; }
        public List<ContentItemDto> ContentItems { get; set; }
        public List<TagDto> Tags { get; set; }

        public static async Task<ReadingDto> BuildReadingDto(Reading reading, DbContext context, IEnumerable<StudentWord> studentWords = null)
        {
            var dto = new ReadingDto(reading);

            if (studentWords != null)
            {
                SetWordStatuses(dto, reading, studentWords);
            }

            if (reading.Tags != null)
            {
                foreach (var tag in reading.Tags)
                {
                    dto.Tags.Add(new TagDto(tag.Tag.Id, tag.Tag.Value, tag.Tag.IsDefault));
                }
            }

            foreach (var item in reading.ContentItems)
            {
                if (item.AnnotationId == null)
                {
                    dto.ContentItems.Add(new ContentItemDto(item.Id, item.BodyIndex,
                        item.Value));
                }
                else
                {
                    // add defined content items
                    var annotation = await context.FindAsync<Annotation>(item.AnnotationId);
                    var ci = new ContentItemDto(
                        item.Id,
                        item.BodyIndex,
                        item.Value,
                        annotation.Value,
                        item.WordId,
                        item.AnnotationId,
                        item.AnnotationContextId);

                    ci.DefinitionUsedByWordsCount = context.Set<ContentItem>().Count(x => x.AnnotationId == item.AnnotationId);
                    ci.DefinitionUsedByStudentsCount = context.Set<StudentWord>().Count(x => x.AnnotationId == item.AnnotationId);

                    dto.ContentItems.Add(ci);


                }
            }

            return dto;
        }

        public static IEnumerable<ReadingDto> BuildReadingDtos(IEnumerable<Reading> readings, IEnumerable<StudentWord> studentWords = null)
        {
            var result = new List<ReadingDto>();

            foreach (var reading in readings)
            {
                var dto = new ReadingDto(reading);

                if (studentWords != null)
                {
                    SetWordStatuses(dto, reading, studentWords);
                }

                if (reading.Tags != null)
                {
                    foreach (var tag in reading.Tags)
                    {
                        dto.Tags.Add(new TagDto(tag.Tag.Id, tag.Tag.Value, tag.Tag.IsDefault));
                    }
                }

                result.Add(dto);
            }

            return result;
        }

        private static void SetWordStatuses(ReadingDto dto, Reading reading, IEnumerable<StudentWord> studentWords)
        {
            dto.KnownWords = reading.ContentItems.Count(c => c.WordId != null &&
                studentWords.Where(w => w.IsKnown).Select(w => w.WordEntryId).Contains(c.WordId));

            dto.InProgressWords = reading.ContentItems.Count(c => c.WordId != null &&
                            studentWords.Where(w => !w.IsKnown).Select(w => w.WordEntryId).Contains(c.WordId));

            dto.NewWords = reading.ContentItems.Count(c => c.WordId != null &&
                            !studentWords.Select(w => w.WordEntryId).Contains(c.WordId));
        }
    }
}
