using VkCore.Builders;
using VkCore.Constants;
using VkCore.Models.Word;
using Xunit;

namespace VkCore.Test.Builders
{
    public class DefinitionDtoBuilderShould
    {
        [Fact]
        public void SetIsEditableFalseWhenAnnotationIsAssignedAndUserCreated()
        {
            var a = new Annotation("1");
            a.Source = DefinitionSourceTypes.UserCode;
            a.AddContext("test", "1");

            var result = DefinitionDtoBuilder.FromAnnotation(a, null);

            Assert.False(result.IsEditable);
        }

        [Fact]
        public void SetIsEditableTrueWhenAnnotationIsNotUserCreatedAndInUse()
        {
            var a = new Annotation("1");
            a.Source = "some-non-user-source";
            a.AddContext("test", "1");

            var result = DefinitionDtoBuilder.FromAnnotation(a, null);

            Assert.True(result.IsEditable);
        }

        [Fact]
        public void SetIsEditableTrueWhenAnnotationIsNotUserCreatedAndNotInUse()
        {
            var a = new Annotation("1");
            a.Source = "some-non-user-source";

            var result = DefinitionDtoBuilder.FromAnnotation(a, null);

            Assert.True(result.IsEditable);
        }

        [Fact]
        public void SetIsEditableTrueWhenAnnotationIsUserCreatedButNotInUse()
        {
            var a = new Annotation("1");
            a.Source = DefinitionSourceTypes.UserCode;

            var result = DefinitionDtoBuilder.FromAnnotation(a, null);

            Assert.True(result.IsEditable);
        }
    }
}
