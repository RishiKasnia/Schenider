using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thesaurus.Services.BusinessEntities

{
    public class Word
    {
        /// <summary>
        /// Unique Id of entry 
        /// </summary>
        public int WordId { get; set; }

        /// <summary>
        /// Word for Dictionary
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Description of the word
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Associated synonyms
        /// </summary>
        public List<Synonym> Synonyms { get; set; }
    }
}
