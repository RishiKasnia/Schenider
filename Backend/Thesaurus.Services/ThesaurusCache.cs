using System.Collections.Generic;
using System.Linq;

namespace Thesaurus
{
    public static class ThesaurusCache
    {
        //create object
        private static int initialCapacity = 82765;
        private static int maxEditDistanceDictionary = 2; //maximum edit distance per dictionary precalculation
        private static SymSpell symSpell;

        static ThesaurusCache()
        {
            symSpell = new SymSpell(initialCapacity, maxEditDistanceDictionary,3);
        }


        static public void LoadDictonaryFromDB(List<string> wordsList, bool forceRefresh)
        {
            if (forceRefresh)
            {
                symSpell = new SymSpell(initialCapacity, maxEditDistanceDictionary, 3);
            }
            foreach (string word in wordsList)
            {
                AddWordToTextDictionary(word, 11);
            }
        }

        /// <summary>
        /// Get all related suggestions
        /// </summary>
        /// <param name="inputTerm">lookup suggestions for single-word input string</param>
        /// <returns></returns>
        public static List<string> GetWordSuggestions(string inputTerm)
        {
            int maxEditDistanceLookup = 1; //max edit distance per lookup (maxEditDistanceLookup<=maxEditDistanceDictionary)
            var suggestionVerbosity = SymSpell.Verbosity.Closest; //Top, Closest, All
            var allSuggestions = symSpell.Lookup(inputTerm, suggestionVerbosity, maxEditDistanceLookup);

            return allSuggestions.Select(suggestion => suggestion.term).ToList();
        }

        public static bool AddWordToTextDictionary(string word, long count)
        {
            bool res = symSpell.CreateDictionaryEntry(word, count);
            return res;
        }
    }
}
