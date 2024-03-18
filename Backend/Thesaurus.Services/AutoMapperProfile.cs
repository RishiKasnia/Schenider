using AutoMapper;
using Thesaurus.DAL.Entities;
using Thesaurus.Services.BusinessEntities;

namespace Thesaurus.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<WordDTO, Word>()
               .ForMember(dest => dest.Synonyms, opt => opt.MapFrom(src => src.Synonyms)).ReverseMap();

            CreateMap<Synonym, SynonymDTO>().ReverseMap(); 
        }
    }
}
