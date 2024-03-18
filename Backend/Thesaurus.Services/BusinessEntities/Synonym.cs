using System.ComponentModel.DataAnnotations;

namespace Thesaurus.Services.BusinessEntities
{
    public class Synonym
    {
        /// <summary>
        /// Unique Id of Synonym 
        /// </summary>
        public int SynonymId { get; set; }


        /// <summary>
        /// Unique Id of Word 
        /// </summary>
        public int WordId { get; set; }

        /// <summary>
        /// Synonym Title
        /// </summary>
        [Required]
        public string Title { get; set; }
    
    }
}
