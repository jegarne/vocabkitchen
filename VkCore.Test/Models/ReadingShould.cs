using System;
using System.Linq;
using VkCore.Builders;
using VkCore.Constants;
using VkCore.Models.ReadingModel;
using VkCore.Models.Word;
using VkCore.Requests.ReadingRequest;
using Xunit;

namespace VkCore.Test.Models
{
    public class ReadingShould
    {
        [Fact]
        public void AppendContentCorrectly()
        {
            var sut = new Reading("", "012");
            sut.AppendContent("345");

            var items = sut.ContentItems.ToList();
            Assert.Equal(0, items[0].FirstIndex);
            Assert.Equal(5, items[0].LastIndex);
            Assert.Equal("012345", items[0].Value);
        }

        [Fact]
        public void AppendContentAtIndexCorrectly()
        {
            var sut = new Reading("", "012");
            sut.AppendContent("45");
            sut.InsertContent(3, "3");

            var items = sut.ContentItems.ToList();
            Assert.Equal(0, items[0].FirstIndex);
            Assert.Equal(5, items[0].LastIndex);
            Assert.Equal("012345", items[0].Value);
        }

        [Fact]
        public void MaintainConsecutiveIndexesAfterInserts()
        {
            var sut = new Reading("", "01");
            sut.AppendContent("23");
            sut.InsertContent(4, "4");
            sut.InsertContent(0, "0");
            sut.InsertContent(6, "5");

            var items = sut.ContentItems.ToList();
            for (int i = 0; i < items.Count() - 1; i++)
            {
                var end = items[i].LastIndex;
                var beginNext = items[i + 1].FirstIndex;
                Assert.Equal(end + 1, beginNext);
            }

            Assert.Equal("0012345", sut.Text);
        }

        [Fact]
        public void BuildTextCorrectly()
        {
            var sut = new Reading("", "My ");
            sut.AppendContent(" is Fred.");
            sut.InsertContent(3, "name");

            Assert.Equal("My name is Fred.", sut.Text);
        }

        [Fact]
        public void InsertADefinition()
        {
            var sut = new Reading("", "01234");

            var word1 = new WordEntry("");
            var word2 = new WordEntry("");
            var word3 = new WordEntry("");

            sut.InsertDefinition(1, 1, word1.Id, "", "");
            sut.InsertDefinition(2, 2, word2.Id, "", "");
            sut.InsertDefinition(3, 3, word3.Id, "", "");

            var items = sut.ContentItems.ToList();
            Assert.Equal(0, items[0].BodyIndex);
            Assert.Equal(0, items[0].FirstIndex);
            Assert.Equal(0, items[0].LastIndex);
            Assert.Equal("0", items[0].Value);
            Assert.Null(items[0].WordId);

            Assert.Equal(1, items[1].BodyIndex);
            Assert.Equal(1, items[1].FirstIndex);
            Assert.Equal(1, items[1].LastIndex);
            Assert.Equal("1", items[1].Value);
            Assert.Equal(word1.Id, items[1].WordId);


            Assert.Equal(2, items[2].BodyIndex);
            Assert.Equal(2, items[2].FirstIndex);
            Assert.Equal(2, items[2].LastIndex);
            Assert.Equal("2", items[2].Value);
            Assert.Equal(word2.Id, items[2].WordId);


            Assert.Equal(3, items[3].BodyIndex);
            Assert.Equal(3, items[3].FirstIndex);
            Assert.Equal(3, items[3].LastIndex);
            Assert.Equal("3", items[3].Value);
            Assert.Equal(word3.Id, items[3].WordId);

            Assert.Equal(4, items[4].BodyIndex);
            Assert.Equal(4, items[4].FirstIndex);
            Assert.Equal(4, items[4].LastIndex);
            Assert.Equal("4", items[4].Value);
            Assert.Null(items[4].WordId);
        }

        [Theory]
        [InlineData(0, 9)]
        [InlineData(0, 0)]
        [InlineData(9, 9)]
        [InlineData(1, 8)]
        public void RewrapDefinitionsCorrectly(int startIndex, int endIndex)
        {
            var sut = new Reading("", "0123456789");

            sut.InsertDefinition(1, 3, new WordEntry("").Id, "", "");
            Assert.Equal("0123456789", sut.Text);

            sut.InsertDefinition(2, 4, new WordEntry("").Id, "", "");
            Assert.Equal("0123456789", sut.Text);

            sut.InsertDefinition(startIndex, endIndex, new WordEntry("").Id, "", "");
            Assert.Equal("0123456789", sut.Text);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(9, 9)]
        public void NotAddEmptyContentItemsWhenInsertingDefinitionOnEdgeOfContent(int startIndex, int endIndex)
        {
            var sut = new Reading("", "0123456789");
            sut.InsertDefinition(startIndex, endIndex, new WordEntry("").Id, "", "");

            Assert.Equal(2, sut.ContentItems.Count());
        }

        [Fact]
        public void DeleteTheContentItemAtAnExactIndexRange()
        {
            var sut = new Reading("", "012");
            sut.InsertDefinition(2, 2, new WordEntry("").Id, "", "");
            sut.Delete(2, 2);
            Assert.Single(sut.ContentItems);
            Assert.Equal(0, sut.ContentItems.First().BodyIndex);
        }

        [Fact]
        public void DeleteARangeFromInsideAContentItem()
        {
            var sut = new Reading("", "012");
            sut.InsertDefinition(2, 2, new WordEntry("").Id, "", "");
            sut.Delete(1, 1);

            Assert.Equal("02", sut.Text);
        }

        [Fact]
        public void DeleteARangeThatFallsAcrossContentItems()
        {
            var sut = new Reading("", "012345");
            sut.InsertDefinition(2, 2, new WordEntry("").Id, "", "");
            sut.InsertDefinition(3, 3, new WordEntry("").Id, "", "");
            sut.InsertDefinition(4, 4, new WordEntry("").Id, "", "");
            sut.Delete(2, 4);

            Assert.Equal("015", sut.Text);
        }

        [Fact]
        public void SetBodyIndexesCorrectlyAfterDeletion()
        {
            var sut = new Reading("", "012345");
            sut.InsertDefinition(2, 2, new WordEntry("").Id, "", "");
            sut.InsertDefinition(3, 3, new WordEntry("").Id, "", "");
            sut.InsertDefinition(4, 4, new WordEntry("").Id, "", "");
            sut.Delete(2, 4);

            Assert.Contains(sut.ContentItems, i => i.BodyIndex == 0);
            Assert.Contains(sut.ContentItems, i => i.BodyIndex == 1);
            Assert.Contains(sut.ContentItems, i => i.BodyIndex == 2);
        }

        [Fact]
        public void BackspaceTextToEmpty()
        {
            var sut = new Reading("", "0");
            sut.AppendContent("1");
            sut.Delete(0, 1);

            Assert.Equal("", sut.Text);
        }

        [Fact]
        public void SetIndexesCorrectly()
        {
            var sut = new Reading("", "0");
            sut.Delete(0, 0);
            sut.AppendDefinition("01", "0");
            sut.AppendDefinition("23", "1");
            sut.AppendDefinition("45", "2");

            var items = sut.ContentItems.ToList();
            Assert.Equal(0, items[0].FirstIndex);
            Assert.Equal(1, items[0].LastIndex);

            Assert.Equal(2, items[1].FirstIndex);
            Assert.Equal(3, items[1].LastIndex);

            Assert.Equal(4, items[2].FirstIndex);
            Assert.Equal(5, items[2].LastIndex);
        }

        [Fact]
        public void SetIndexesConsecutively()
        {
            var sut = new Reading("", "0");
            sut.Delete(0, 0);
            sut.AppendDefinition("01234", "0");
            sut.AppendDefinition("56789", "2");
            sut.AppendDefinition("asdfadsfdsljkhakjhjjkljhasdjfsjkdjkhjjkjkaskjldfjkd", "2");
            sut.AppendDefinition("jkhakjhjjkljhasdjfsjkdjkhjjkjkaskjldfjkd", "2");
            sut.AppendDefinition("hjjkljhasdjfsjkdjkhjjkjkaskjldfjkd", "3");
            sut.AppendDefinition("fdsljkhakjhjjkljhasdjfsjkdjkhjjkjkaskjldfjkd", "4");
            sut.AppendDefinition("dfadsfdsa", "4");

            var items = sut.ContentItems.ToList();
            for (int i = 0; i < items.Count() - 1; i++)
            {
                var end = items[i].LastIndex;
                var beginNext = items[i + 1].FirstIndex;
                Assert.Equal(end + 1, beginNext);
            }
        }


        [Fact]
        public void InsertNewlinesCorrectly()
        {
            var newLine = "\r\n\r\n";
            var sut = new Reading("", "0123456");
            sut.InsertContent(3, newLine);
            sut.InsertContent(7, newLine);

            Assert.Equal("012\n\n34\n\n56", sut.Text);

        }

        [Fact]
        public void CleanupContentItems()
        {
            var sut = new Reading("", "01");
            sut.InsertContent(2, "23");
            sut.InsertContent(4, "45");

            sut.CleanupContentItems();

            Assert.Single(sut.ContentItems);
            Assert.Equal("012345", sut.Text);
        }

        [Fact]
        public void CleanupContentItemsWithDefinitions()
        {
            var sut = new Reading("", "01");
            var def = new WordEntry("1");

            var def1 = new DefinitionBuilder(def.Id);
            def1.AddExampleSentence("01", sut.Id);
            def1.SetContent("one", "noun");
            def1.SetSource(DefinitionSourceTypes.UserCode, "userId");
            def.AddAnnotation(def1.GetAnnotation());

            sut.InsertDefinition(1, 1, def.Id, "", "");
            sut.InsertContent(2, "23");
            sut.InsertContent(4, "45");

            sut.CleanupContentItems();

            Assert.Equal(3, sut.ContentItems.Count());
            Assert.Equal("012345", sut.Text);
        }

        [Fact]
        public void CleanupContentItemsWithDefinitions2()
        {
            var sut = new Reading("", "01");
            var def = new WordEntry("1");

            var def1 = new DefinitionBuilder(def.Id);
            def1.AddExampleSentence("01", sut.Id);
            def1.SetContent("one", "noun");
            def1.SetSource(DefinitionSourceTypes.UserCode, "userId");
            def.AddAnnotation(def1.GetAnnotation());

            sut.InsertDefinition(1, 1, def.Id, "", "");
            sut.InsertContent(2, "23");
            sut.InsertContent(4, "45");
            sut.InsertContent(0, "0");

            sut.CleanupContentItems();

            Assert.Equal("0012345", sut.Text);
        }

        [Fact]
        public void InsertDefinitionFromDefinitionRequest()
        {
            var sut = new Reading("", "0123. 6789.");
            var request = new AddDefinitionRequest()
            {
                ContentItemStartIndex = 0,
                ContentItemEndIndex = 0,
                Start = 1,
                End = 2
            };

            var word = new WordEntry("");
            var result = sut.InsertDefinition(request, word.Id, "", "");

            Assert.Equal("0", sut.ContentItems.First(x => x.BodyIndex == 0).Value);
            Assert.Equal("12", sut.ContentItems.First(x => x.BodyIndex == 1).Value);
            Assert.Equal(word.Id, sut.ContentItems.First(x => x.BodyIndex == 1).WordId);
            Assert.Equal("3. 6789.", sut.ContentItems.First(x => x.BodyIndex == 2).Value);
        }

        [Fact]
        public void GetWordContext()
        {
            var sut = new Reading("", "0123. 678?");
            var request = new AddDefinitionRequest()
            {
                ContentItemStartIndex = 0,
                ContentItemEndIndex = 0,
                Start = 7,
                End = 7
            };

            var result = sut.GetWordContext(request);

            Assert.Equal("678?", result);
        }
    }
}
