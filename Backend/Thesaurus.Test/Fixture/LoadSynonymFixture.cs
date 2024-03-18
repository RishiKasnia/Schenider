using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Thesaurus.DAL.Entities;
using Thesaurus.Test.Common;

namespace Thesaurus.Test.Fixture
{
    [ExcludeFromCodeCoverage]
    public class LoadSynonymFixture
    {
        public List<SynonymDTO> GetSynonyms(int wordId)
        {
            return SynonymSampleData.Data(wordId);
        }
    }
}
