using System.Collections.Generic;

namespace Thesaurus.Services.BusinessEntities
{

    /// <summary>
    /// Class to represent a paged output support server side paging
    /// </summary>
    public class PagedOutput
    {
        /// <summary>
        /// total number of items in the DB
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// total pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Current page showing
        /// </summary>
        public int CurrentPage { get; set; }


        /// <summary>
        /// List of word objects
        /// </summary>
        public List<Word> Words { get; set; }
    }
}
