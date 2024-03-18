using System;
using System.Collections.Generic;

namespace Thesaurus.DAL.Entities
{
    public class Thesaurus
    {
        public string Word { get; set; }

        public List<SynonymDTO> Synonyms { get; set; }

    }
}
