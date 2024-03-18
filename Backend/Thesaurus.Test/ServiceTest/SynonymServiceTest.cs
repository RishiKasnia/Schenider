using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thesaurus.DAL;
using Thesaurus.DAL.Entities;
using Thesaurus.DAL.Interfaces;
using Thesaurus.Services;
using Thesaurus.Services.BusinessEntities;
using Thesaurus.Test.Fixture;
using Xunit;

namespace Thesaurus.Test
{
    public class SynonymServiceTest:IClassFixture<LoadSynonymFixture>,IClassFixture<MapperFixture>
    {


        private readonly Mock<ISynonymRepository> _iSynonymRepositoryMoq;

        private readonly Mock<IUnitOfWork> _iUOWMoq;

        private readonly Mock<IDalSession> _iDalSessionMoq;

        private readonly LoadSynonymFixture _loadFixture;
        private readonly IMapper _iMapper;


        public SynonymServiceTest(LoadSynonymFixture loadFixture, MapperFixture mapperFixture)
        {

            _iSynonymRepositoryMoq = new Mock<ISynonymRepository>();
            _iDalSessionMoq = new Mock<IDalSession>();
            _loadFixture = loadFixture;
            _iMapper = mapperFixture.GetMapper();
            _iUOWMoq = new Mock<IUnitOfWork>();
            
        }

        [Fact]
        public async Task GetByIdAsync_Test_Returns_Synonym()
        {
            int wordId = 1;
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object).Verifiable();
            _iSynonymRepositoryMoq.Setup(moq=> moq.GetByIdAsync(1)).ReturnsAsync(_loadFixture.GetSynonyms(wordId).First(x => x.WordId == wordId)).Verifiable();
            SynonymService service = new SynonymService(_iDalSessionMoq.Object, _iSynonymRepositoryMoq.Object, _iMapper);
            var result = await service.GetByIdAsync(1);

            Assert.True(result.WordId == 1);
            Assert.True(result.Title == "UnitTestSynmDTOTitle1");
        }


        [Fact]
        public async Task GetByIdAsync_Test_Returns_Exception_When_Id_Is_Zero()
        {
            SynonymDTO synonym = null;
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iSynonymRepositoryMoq.Setup(moq => moq.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(synonym).Verifiable();
            SynonymService service = new SynonymService(_iDalSessionMoq.Object, _iSynonymRepositoryMoq.Object, _iMapper);
            await Assert.ThrowsAsync<HttpStatusException>(() => service.GetByIdAsync(0));

        }
        [Fact]
        public async Task AddAsync_Test_Returns_Synonyms()
        {
            int wordId = 1;

            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iSynonymRepositoryMoq.Setup(moq => moq.AddAsync(It.IsAny<SynonymDTO>())).ReturnsAsync(1).Verifiable();
            _iSynonymRepositoryMoq.Setup(moq => moq.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_loadFixture.GetSynonyms(wordId).First(x => x.WordId == 1)).Verifiable();

            SynonymService service = new SynonymService(_iDalSessionMoq.Object, _iSynonymRepositoryMoq.Object, _iMapper);
            var result = await service.AddAsync(new Synonym() { SynonymId=1, WordId = 1 });

            Assert.True(result.WordId == 1);
        }

        [Fact]
        public async Task Delete_All_By_Word_Test_Returns_True()
        {
            int wordId = 1;

            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iSynonymRepositoryMoq.Setup(moq => moq.DeleteAllByWordIdAsync(It.IsAny<int>())).ReturnsAsync(1).Verifiable();

            SynonymService service = new SynonymService(_iDalSessionMoq.Object, _iSynonymRepositoryMoq.Object, _iMapper);
            var result = await service.DeleteAllByWordIdAsync(wordId);

            Assert.True(result);
        }
        [Fact]
        public async Task Delete_Async_Test_Returns_True()
        {
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iSynonymRepositoryMoq.Setup(moq => moq.DeleteAsync(It.IsAny<int>())).ReturnsAsync(1).Verifiable();

            SynonymService service = new SynonymService(_iDalSessionMoq.Object, _iSynonymRepositoryMoq.Object, _iMapper);
            var result = await service.DeleteAsync(1);

            Assert.True(result);
        }


        [Fact]
        public async Task UpdateAsync_Test_Returns_Word()
        {
            int wordId = 1;
            string synTitle = "UnitTestSynmDTOTitle1";
            Synonym returnVal = new Synonym() { SynonymId = 1, WordId = wordId, Title= synTitle };
           
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iSynonymRepositoryMoq.Setup(moq => moq.UpdateAsync(It.IsAny<SynonymDTO>())).ReturnsAsync(1).Verifiable();
            _iSynonymRepositoryMoq.Setup(moq => moq.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_loadFixture.GetSynonyms(wordId).First(x => x.WordId == 1)).Verifiable();

            SynonymService service = new SynonymService(_iDalSessionMoq.Object, _iSynonymRepositoryMoq.Object, _iMapper);
            var result = await service.UpdateAsync(returnVal);

            Assert.True(result.Title == synTitle);
        }

        [Fact]
        public async Task UpdateRangeAsync_Test_Returns_Word()
        {
            int wordId = 1;
            string synTitle = "UnitTestSynmDTOTitle1";
            List<Synonym> returnVal = new List<Synonym>(){ new Synonym() { SynonymId = 1, WordId = wordId, Title = synTitle } };

            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iSynonymRepositoryMoq.Setup(moq => moq.UpdateRangeAsync(It.IsAny<List<SynonymDTO>>())).ReturnsAsync(1).Verifiable();
            _iSynonymRepositoryMoq.Setup(moq => moq.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_loadFixture.GetSynonyms(wordId).First(x => x.WordId == 1)).Verifiable();

            SynonymService service = new SynonymService(_iDalSessionMoq.Object, _iSynonymRepositoryMoq.Object, _iMapper);
            var result = await service.UpdateRangeAsync(returnVal);

            Assert.True(result);
        }

    }
}
