using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkCore.Builders;
using VkCore.Constants;
using VkCore.Dtos;
using VkCore.Interfaces;

namespace VkInfrastructure.DefinitionSources
{
    public class OwlBotApi : IDefinitionSource
    {
        private string _baseUrl = "https://owlbot.info/api/v3/dictionary/";

        public async Task<IEnumerable<DefinitionDto>> GetEntries(string word)
        {
            var result = new List<DefinitionDto>();

            try
            {
                dynamic apiResult = await OwlApiSearchUrl(word).GetJsonAsync();
                foreach (var d in apiResult.definitions)
                {
                    var builder = new DefinitionDtoBuilder();

                    builder.SetContent(d.definition, d.type, d.image_url);
                    builder.SetSource(DefinitionSourceTypes.OwlBotCode);

                    result.Add(builder.GetDto());
                }
            }
            catch (FlurlHttpException)
            {
                // TODO: log this
            }

            return result;
        }

        private IFlurlRequest OwlApiSearchUrl(string word)
        {
            return $"{_baseUrl}{word}"
                .WithHeader("Authorization", "Token here");
        }
    }
}
