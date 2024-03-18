using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Thesaurus.DAL;
using Thesaurus.DAL.Entities;
using Thesaurus.DAL.Interfaces;
using Thesaurus.Services;
using Thesaurus.Services.BusinessEntities;
using Thesaurus.Test.Fixture;
using Xunit;

namespace Thesaurus.Test
{
    public class WordServiceTest:IClassFixture<LoadWordFixture>,IClassFixture<MapperFixture>
    {

        private readonly Mock<IWordRepository> _iWordRepoMoq;

        private readonly Mock<IUnitOfWork> _iUOWMoq;

        private readonly Mock<IMemoryCache> _iMemoryCacheMoq;
        private readonly Mock<IDalSession> _iDalSessionMoq;

        private readonly LoadWordFixture _loadFixture;
        private readonly IMapper _iMapper;


        public WordServiceTest(LoadWordFixture loadFixture, MapperFixture mapperFixture)
        {
            _iWordRepoMoq = new Mock<IWordRepository>();
            _iMemoryCacheMoq = new Mock<IMemoryCache>();
            _iDalSessionMoq = new Mock<IDalSession>();
            _loadFixture = loadFixture;
            _iMapper = mapperFixture.GetMapper();
            _iUOWMoq = new Mock<IUnitOfWork>();
            
        }

        [Fact]
        public async Task GetAllWords_Test_Returns_Words()
        {
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object).Verifiable();
            _iWordRepoMoq.Setup(moq=> moq.GetAllAsync(1,1)).ReturnsAsync(_loadFixture.GetWords()).Verifiable();
            WordService service = new WordService(_iDalSessionMoq.Object, _iWordRepoMoq.Object, _iMemoryCacheMoq.Object, _iMapper);
            var result = await service.GetAllAsync(1,1);

            Assert.True(result.Words.Count == 3);// Loaded 3 words in fixture
            Assert.True(result.Words[0].Synonyms.Count == 3);// Loaded 3 synonym in fixture
            Assert.True(result.Words[0].Title == "UnitTestWordDTOTitle1");
        }

        [Fact]
        public async Task GetByIdAsync_Test_Returns_Word()
        {
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iWordRepoMoq.Setup(moq => moq.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_loadFixture.GetWords().First(x=>x.WordId==1)).Verifiable();
            WordService service = new WordService(_iDalSessionMoq.Object, _iWordRepoMoq.Object, _iMemoryCacheMoq.Object, _iMapper);
            var result = await service.GetByIdAsync(1);

            Assert.True(result.WordId==1);
        }

        [Fact]
        public async Task GetByIdAsync_Test_Returns_Exception_When_Id_Is_Zero()
        {
            WordDTO word = null;
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iWordRepoMoq.Setup(moq => moq.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(word).Verifiable();
            WordService service = new WordService(_iDalSessionMoq.Object, _iWordRepoMoq.Object, _iMemoryCacheMoq.Object, _iMapper);
            await Assert.ThrowsAsync<HttpResponseException>(() => service.GetByIdAsync(0));

        }

        [Fact]
        public async Task GetWordByTitleAsync_Test_Returns_Word()
        {
            string wordTitle = "UnitTestWordDTOTitle1";
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iWordRepoMoq.Setup(moq => moq.GetWordByTitleAsync(It.IsAny<string>())).ReturnsAsync(_loadFixture.GetWords().First(x => x.Title.Equals(wordTitle))).Verifiable();

            WordService service = new WordService(_iDalSessionMoq.Object, _iWordRepoMoq.Object, _iMemoryCacheMoq.Object, _iMapper);
            var result = await service.GetWordByTitleAsync(wordTitle);

            Assert.True(result.Title == wordTitle);
        }
        
        [Fact] 
        public async Task GetWordByTitleAsync_Test_Returns_Excepion_when_Title_Is_Empty()
        {
            string wordTitle = "UnitTestWordDTOTitle1";
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iWordRepoMoq.Setup(moq => moq.GetWordByTitleAsync(It.IsAny<string>())).ReturnsAsync(_loadFixture.GetWords().First(x => x.Title.Equals(wordTitle))).Verifiable();

            WordService service = new WordService(_iDalSessionMoq.Object, _iWordRepoMoq.Object, _iMemoryCacheMoq.Object, _iMapper);
            await Assert.ThrowsAsync<HttpStatusException>(() => service.GetWordByTitleAsync(string.Empty));
        }

        [Fact(Skip = "need to manage caching better")]
        public async Task AddAsync_Test_Returns_Word()
        {
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iWordRepoMoq.Setup(moq => moq.AddAsync(It.IsAny<WordDTO>())).ReturnsAsync(1).Verifiable();
            _iWordRepoMoq.Setup(moq => moq.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_loadFixture.GetWords().First(x => x.WordId == 1)).Verifiable();

            WordService service = new WordService(_iDalSessionMoq.Object, _iWordRepoMoq.Object, _iMemoryCacheMoq.Object, _iMapper);
            var result = await service.AddAsync(new Word() { WordId = 1 });

            Assert.True(result.WordId == 1);
        }

        [Fact]
        public async Task AddAsync_Test_Returns_Exception_When_Word_Is_Empty()
        {
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iWordRepoMoq.Setup(moq => moq.AddAsync(It.IsAny<WordDTO>())).ReturnsAsync(1).Verifiable();
            _iWordRepoMoq.Setup(moq => moq.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_loadFixture.GetWords().First(x => x.WordId == 1)).Verifiable();

            WordService service = new WordService(_iDalSessionMoq.Object, _iWordRepoMoq.Object, _iMemoryCacheMoq.Object, _iMapper);
            await Assert.ThrowsAsync<HttpStatusException>(() => service.AddAsync(null));
        }

        [Fact]
        public async Task AddAsync_Test_Returns_Exception_When_AddingExisting_Word()
        {
            string wordTitle = "UnitTestWordDTOTitle1";
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);

            _iWordRepoMoq.Setup(moq => moq.AddAsync(It.IsAny<WordDTO>())).ReturnsAsync(1).Verifiable();
            _iWordRepoMoq.Setup(moq => moq.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_loadFixture.GetWords().First(x => x.WordId == 1)).Verifiable();
            _iWordRepoMoq.Setup(moq => moq.GetWordByTitleAsync(It.IsAny<string>())).ReturnsAsync(_loadFixture.GetWords().First(x => x.Title.Equals(wordTitle))).Verifiable();

            WordService service = new WordService(_iDalSessionMoq.Object, _iWordRepoMoq.Object, _iMemoryCacheMoq.Object, _iMapper);

            await Assert.ThrowsAsync<HttpStatusException>(() => service.AddAsync(new Word() { Title = wordTitle }));
        }

        [Fact (Skip = "need to manage caching")]
        public async Task DeleteAsync_Test_Deletes_Word()
        {
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iWordRepoMoq.Setup(moq => moq.DeleteAsync(It.IsAny<int>())).ReturnsAsync(1).Verifiable();
            _iWordRepoMoq.Setup(moq => moq.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_loadFixture.GetWords().First(x => x.WordId == 1)).Verifiable();


            WordService service = new WordService(_iDalSessionMoq.Object, _iWordRepoMoq.Object, _iMemoryCacheMoq.Object, _iMapper);
            var result = await service.DeleteAsync(1);

            Assert.True(result );
        }

        [Fact]
        public async Task DeleteAsync_Test_Throws_Exception_when_Deleting_Non_Existing_Word()
        {
            WordDTO searchDTO = null;
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iWordRepoMoq.Setup(moq => moq.DeleteAsync(It.IsAny<int>())).ReturnsAsync(1).Verifiable();
            _iWordRepoMoq.Setup(moq => moq.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(searchDTO).Verifiable();

            WordService service = new WordService(_iDalSessionMoq.Object, _iWordRepoMoq.Object,_iMemoryCacheMoq.Object, _iMapper);

            await Assert.ThrowsAsync<HttpStatusException>(() => service.DeleteAsync(0));

        }

        [Fact]
        public async Task UpdateAsync_Test_Throws_Exception_when_updating_Non_Existing_Word()
        {
            WordDTO inputEntity = new WordDTO();
            Word updateEntity = new Word();
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iWordRepoMoq.Setup(moq => moq.UpdateAsync(It.IsAny<WordDTO>())).ReturnsAsync(1).Verifiable();
            _iWordRepoMoq.Setup(moq => moq.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(inputEntity).Verifiable();

            WordService service = new WordService(_iDalSessionMoq.Object, _iWordRepoMoq.Object, _iMemoryCacheMoq.Object, _iMapper);

            await Assert.ThrowsAsync<HttpStatusException>(() => service.UpdateAsync(updateEntity));

        }

        [Fact (Skip = "need to manage caching")]
        public async Task UpdateAsync_Test__Word()
        {
            WordDTO searchDTO = new WordDTO() {WordId=5 };
            _iDalSessionMoq.Setup(moq => moq.UnitOfWork).Returns(_iUOWMoq.Object);
            _iWordRepoMoq.Setup(moq => moq.UpdateAsync(It.IsAny<WordDTO>())).ReturnsAsync(1).Verifiable();
            _iWordRepoMoq.Setup(moq => moq.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(searchDTO).Verifiable();

            WordService service = new WordService(_iDalSessionMoq.Object, _iWordRepoMoq.Object, _iMemoryCacheMoq.Object, _iMapper);

            var result = await service.UpdateAsync(new Word() { WordId = 5});

            Assert.True(result.WordId== searchDTO.WordId);

        }

    }
}
