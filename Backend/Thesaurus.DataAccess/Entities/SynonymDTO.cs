namespace Thesaurus.DAL.Entities
{
    public class SynonymDTO
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
        public string Title { get; set; }
    
    }
}
