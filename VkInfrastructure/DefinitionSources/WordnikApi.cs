using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkCore.Builders;
using VkCore.Dtos;
using VkCore.Interfaces;
using VkInfrastructure.Extensions;

namespace VkInfrastructure.DefinitionSources
{
    public class WordnikApi : IDefinitionSource
    {
        private string _baseUrl = "https://api.wordnik.com/v4/word.json/";
        private string _apiKey = "my api key";
        private string _sourceDictionaries;

        public WordnikApi(string[] sourceDictionaries)
        {
            _sourceDictionaries = string.Join(',', sourceDictionaries);
        }

        public WordnikApi()
        {
            _sourceDictionaries = "all";
        }

        public async Task<IEnumerable<DefinitionDto>> GetEntries(string word)
        {
            var result = new List<DefinitionDto>();

            try
            {
                dynamic apiResult = await SearchUrl(word).GetJsonListAsync();
                foreach (var d in apiResult)
                {
                    if (!DynamicUtilities.HasProperty(d, "text"))
                    {
                        // some definitions were missing
                        // the text property, so let's skip those
                        // as they won't have a definition
                        continue;
                    }

                    var builder = new DefinitionDtoBuilder();
                    string definition = d.text;
                    string partOfSpeech = null;
                    if (DynamicUtilities.HasProperty(d, "partOfSpeech"))
                    {
                        partOfSpeech = d.partOfSpeech;
                    }
                    builder.SetContent(definition.RemoveXml(), partOfSpeech);


                    if (DynamicUtilities.HasProperty(d, "sourceDictionary"))
                    {
                        builder.SetSource(d.sourceDictionary);
                    }

                    result.Add(builder.GetDto());
                }
            }
            catch (FlurlHttpException ex)
            {
                var i = ex;
                // TODO: log this
            }
            catch (Exception ex)
            {
                var i = ex;
            }

            return result;
        }

        private string SearchUrl(string word)
        {
            return $"{_baseUrl}{word}/definitions?limit=200" +
                   $"&includeRelated=false&useCanonical=false&includeTags=false" +
                   $"&sourceDictionaries={_sourceDictionaries}&api_key={_apiKey}";
        }
    }
}
