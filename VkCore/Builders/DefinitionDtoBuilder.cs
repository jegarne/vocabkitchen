using System.Linq;
using VkCore.Constants;
using VkCore.Dtos;
using VkCore.Interfaces;
using VkCore.Models.Word;

namespace VkCore.Builders
{
    public class DefinitionDtoBuilder
    {
        private readonly DefinitionDto _dto;

        public DefinitionDtoBuilder()
        {
            _dto = new DefinitionDto();
        }

        public void SetContent(string definition, string partOfSpeech, string imageUrl = null)
        {
            _dto.Value = definition;
            _dto.PartOfSpeech = partOfSpeech;
            _dto.ImageUrl = imageUrl;
        }

        public void SetSource(string source, string annotationId = null)
        {
            _dto.Source = source;
            _dto.AnnotationId = annotationId;
        }

        public void SetUserSources(IUserUpdateable source)
        {
            _dto.LastUpdateDate = source.UpdateDate.ToString("dd MMMM yyyy");
        }

        public DefinitionDto GetDto()
        {
            return _dto;
        }

        public static DefinitionDto FromAnnotation(Annotation a, string requestingUserId)
        {
            var builder = new DefinitionDtoBuilder();
            builder.SetContent(a.Value, a.PartOfSpeech);
            builder.SetSource(a.Source, a.Id);
            builder.SetUserSources(a);
            var result = builder.GetDto();

            // if an Annotation is user created and used in any readings
            // it should not be editable, everything else you can edit.
            // Unassigned user-created annotations can be edited
            // leaving open the possibility for a "My words" edit page
            if (a.Source == DefinitionSourceTypes.UserCode && a.AnnotationContexts != null && a.AnnotationContexts.Any())
            {
                result.IsEditable = false;
                return result;
            }

            result.IsMine = a.UpdatedBy == requestingUserId;

            return result;
        }
    }
}
