using System.Collections.Generic;

namespace Thesaurus.DAL.Entities
{
    public class WordDTO
    {
        /// <summary>
        /// Unique Id of entry 
        /// </summary>
        public int WordId { get; set; }

        /// <summary>
        /// Word for Dictionary
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the word
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of attached synonyms
        /// </summary>
        public List<SynonymDTO> Synonyms { get; set; }

    }
}
