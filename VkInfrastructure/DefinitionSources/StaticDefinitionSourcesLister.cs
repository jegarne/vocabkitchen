using System.Collections.Generic;
using System.Linq;
using VkCore.Constants;
using VkCore.Models.DefinitionSourceModel;

namespace VkInfrastructure.DefinitionSources
{
    public class StaticDefinitionSourcesLister
    {
        private List<DefinitionSource> _sources = new List<DefinitionSource>();

        public StaticDefinitionSourcesLister()
        {
            _sources.Add(new DefinitionSource() { Code = DefinitionSourceTypes.UserCode, DisplayName = "Vk User Dictionary", AttributionText = null });

            _sources.Add(new DefinitionSource() { Code = DefinitionSourceTypes.OwlBotCode, DisplayName = "OwlBot", AttributionText = null });

            _sources.Add(new DefinitionSource() { Code = "ahd-5", DisplayName = "American Heritage",
                AttributionText = "from The American Heritage® Dictionary of the English Language, 5th Edition." });

            _sources.Add(new DefinitionSource() { Code = "century", DisplayName = "Century", AttributionText = "from The Century Dictionary." });

            _sources.Add(new DefinitionSource() { Code = "gcide", DisplayName = "Websters",
                AttributionText = "from the GNU version of the Collaborative International Dictionary of English." });

            _sources.Add(new DefinitionSource() { Code = "wiktionary", DisplayName = "Wiktionary",
                AttributionText = "from Wiktionary, Creative Commons Attribution/Share-Alike License." });

            _sources.Add(new DefinitionSource() { Code = "wordnet", DisplayName = "Wordnet", AttributionText = "from WordNet 3.0 Copyright 2006 by Princeton University. All rights reserved." });
        }

        public string GetAttribution(string sourceCode)
        {
            if (string.IsNullOrEmpty(sourceCode)) return null;

            return _sources.FirstOrDefault(x => x.Code == sourceCode)?.AttributionText;
        }

        public IEnumerable<DefinitionSource> GetAllSources()
        {
            return _sources;
        }
    }
}
