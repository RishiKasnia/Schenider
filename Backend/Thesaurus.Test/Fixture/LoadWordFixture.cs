using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Thesaurus.DAL.Entities;
using Thesaurus.Test.Common;

namespace Thesaurus.Test.Fixture
{
    [ExcludeFromCodeCoverage]
    public class LoadWordFixture
    {
        public List<WordDTO> GetWords()
        {
            return WordsSampleData.Data();
        }
    }
}
