using Microsoft.AspNetCore.Identity;
using VkInfrastructure.Extensions;
using Xunit;

namespace VkInfrastructure.Test.Extensions
{
    public class IdentityResultExtensionsShould
    {
        [Fact]
        public void ConvertErrorsToDictionary()
        {
            var error = new IdentityError
            {
                Code = "error",
                Description = "something bad happened"
            };
            var sut = IdentityResult.Failed(error);

            var result = sut.GetErrors();

            Assert.Contains(error.Description, result[error.Code]);
        }
    }
}
