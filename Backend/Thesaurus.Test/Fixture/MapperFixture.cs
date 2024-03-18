using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using Thesaurus.Services;

namespace Thesaurus.Test.Fixture
{
    [ExcludeFromCodeCoverage]
    public class MapperFixture
    {
        private IMapper mapper;
        public MapperFixture()
        {
                
        }

        private MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg => {
                cfg.AddProfile(new AutoMapperProfile());
            
            });
            return config;
        }

        public IMapper GetMapper()
        {
            if(mapper==null)
            {
                mapper = new Mapper(InitializeAutoMapper());
            }

            return mapper;
        }
    }
}
