using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Thesaurus.Services;
using Thesaurus.Services.BusinessEntities;

namespace Thesaurus.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    [HttpResponseExceptionFilter]
    public class WordController : ControllerBase
    {
        private readonly IWordService _wordService;
        public WordController(IWordService wordService)
        {
            _wordService = wordService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            if (id > 0)
            {
                var data = await _wordService.GetByIdAsync(id).ConfigureAwait(false);
                if (data == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }
                return Ok(data);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet, Route("title")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                var data = await _wordService.GetWordByTitleAsync(title).ConfigureAwait(false);
                if (data == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }
                return Ok(data);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet, Route("words")]
        public async Task<IActionResult> GetAll(int pageSize, int pageNum)
        {
            if (pageSize <= 0 || pageNum <= 0)
            {
                return BadRequest();
            }
            else
            {
                var data = await _wordService.GetAllAsync(pageSize, pageNum).ConfigureAwait(false);
                if (data == null || data.Words == null || !data.Words.Any())
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }
                return Ok(data);             
            }
        }

        [HttpGet, Route("suggestions")]
        public async Task<IActionResult> GetSuggestions(string title)
        {
            var data = await _wordService.GetTitleSuggestions(title).ConfigureAwait(false);

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Word item)
        {
            if (item != null)
            {
                var data = await _wordService.AddAsync(item).ConfigureAwait(false);

                return Ok(data);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Word item)
        {
            if (item != null)
            {               
                var data = await _wordService.UpdateAsync(item).ConfigureAwait(false);
                return Ok(data);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            if (id > 0)
            {
                var data = await _wordService.DeleteAsync(id).ConfigureAwait(false);
                return Ok(data);
            }
            else
            {
                return BadRequest();
            }
        }
    }

}
