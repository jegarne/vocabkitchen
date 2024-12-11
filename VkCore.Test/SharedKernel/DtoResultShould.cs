using System;
using System.Collections.Generic;
using VkCore.Interfaces;
using VkCore.SharedKernel;
using Xunit;

namespace VkCore.Test.SharedKernel
{
    public class MockClass : IResponseErrorLogger
    {
        public IEnumerable<string> Errors => new List<string>() { "classError" };
    }

    public class DtoResultShould
    {
        [Fact]
        public void AddError()
        {
            var sut = new DtoResult<MockClass>();
            sut.AddError("test", "error");

            Assert.Equal("error", sut.GetErrors()["test"][0]);
        }

        [Fact]
        public void NotAddDuplicateErrors()
        {
            var sut = new DtoResult<MockClass>();
            sut.AddError("test", "error");
            sut.AddError("test", "error");

            Assert.Single(sut.GetErrors()["test"]);
        }

        [Fact]
        public void AddErrorsFromIResponseErrorLogger()
        {
            var mockClass = new MockClass();

            var sut = new DtoResult<MockClass>();
            sut.AddErrors(mockClass);

            Assert.Equal("classError", sut.GetErrors()[String.Empty][0]);
        }

        [Fact]
        public void AddErrorsFromDictionary()
        {
            var dict = new Dictionary<string, List<string>>();
            dict.Add("test", new List<string>() { "error" });

            var sut = new DtoResult<MockClass>();
            sut.AddErrors(dict);

            Assert.Equal("error", sut.GetErrors()["test"][0]);
        }

        [Fact]
        public void SayItHasErrors()
        {
            var sut = new DtoResult<MockClass>();
            sut.AddError("test", "error");

            Assert.True(sut.HasErrors());
        }

        [Fact]
        public void HoldAnObject()
        {
            var mockClass = new MockClass();

            var sut = new DtoResult<MockClass>();
            sut.SetValue(mockClass);

            Assert.Equal(mockClass, sut.Value);
        }
    }
}
