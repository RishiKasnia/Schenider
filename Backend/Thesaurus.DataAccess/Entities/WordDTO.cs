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

        public List<SynonymDTO> Synonyms { get; set; }

    }
}
