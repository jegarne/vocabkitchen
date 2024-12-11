using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using VkInfrastructure.Extensions;
using Xunit;

namespace VkInfrastructure.Test.Extensions
{
    public class DynamicUtilitiesShould
    {
        [Theory]
        [InlineData("{ 'Name': 'Jon Smith', 'Address': { 'City': 'New York', 'State': 'NY' }, 'Age': 42 }", "Name", true)]
        [InlineData("{ 'Name': 'Jon Smith', 'Address': { 'City': 'New York', 'State': 'NY' }, 'Age': 42 }", "Foo", false)]
        public void CheckIfPropertyExists(string json, string property, bool expected)
        {
            var expConverter = new ExpandoObjectConverter();
            dynamic sut = JsonConvert.DeserializeObject<ExpandoObject>(json, expConverter);

            var result = DynamicUtilities.HasProperty(sut, property);

            Assert.Equal(expected, result);
        }
    }
}
