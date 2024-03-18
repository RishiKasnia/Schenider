using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Thesaurus.DAL.Entities;

namespace Thesaurus.Test.Common
{
    [ExcludeFromCodeCoverage]
    public static class WordsSampleData
    {

        public static List<WordDTO> Data()
        {
            List<WordDTO> words = new List<WordDTO> {
                new WordDTO{ WordId = 1,Title = "UnitTestWordDTOTitle1", Description="UnitTestWordDTODescription1", Synonyms=SynonymSampleData.Data(1)},
                new WordDTO{ WordId = 2,Title = "UnitTestWordDTOTitle2", Description="UnitTestWordDTODescription2", Synonyms=SynonymSampleData.Data(2)},
                new WordDTO{ WordId = 3,Title = "UnitTestWordDTOTitle3", Description="UnitTestWordDTODescription3", Synonyms=SynonymSampleData.Data(3)}
            };
            return words;
        }
    }

    [ExcludeFromCodeCoverage]
    public static class SynonymSampleData
    {
        public static List<SynonymDTO> Data(int wordId)
        {
            List<SynonymDTO> synonyms = new List<SynonymDTO> {
                new SynonymDTO{ SynonymId=1, WordId = wordId,Title = "UnitTestSynmDTOTitle1"},
                new SynonymDTO{ SynonymId=2, WordId = wordId,Title = "UnitTestSynmDTOTitle2"},
                new SynonymDTO{ SynonymId=3, WordId = wordId,Title = "UnitTestSynmDTOTitle3"}
            };
            return synonyms;
        }
    }
}
