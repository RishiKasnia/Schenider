using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Thesaurus.Services;
using Thesaurus.Services.BusinessEntities;

namespace Thesaurus.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class SynonymController : ControllerBase
    {
        private readonly ISynonymService _synonymService;
        public SynonymController(ISynonymService synonymService)
        {
            _synonymService = synonymService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            if (id > 0)
            {
                var data = await _synonymService.GetByIdAsync(id).ConfigureAwait(false);
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


        [HttpPost]
        public async Task<IActionResult> Add(Synonym item)
        {
            if (item != null)
            {
                var data = await _synonymService.AddAsync(item).ConfigureAwait(false);

                return Ok(data);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost, Route("synonyms")]
        public async Task<IActionResult> AddRange(List<Synonym> item)
        {
            if (item != null && item.Any())
            {
                var data = await _synonymService.AddRangeAsync(item).ConfigureAwait(false);

                return Ok(data);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Synonym item)
        {
            if (item != null)
            {               
                var data = await _synonymService.UpdateAsync(item).ConfigureAwait(false);
                return Ok(data);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut, Route("synonyms")]
        public async Task<IActionResult> UpdateRange(List<Synonym> item)
        {
            if (item != null)
            {
                var data = await _synonymService.UpdateRangeAsync(item).ConfigureAwait(false);
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
                var data = await _synonymService.DeleteAsync(id).ConfigureAwait(false);
                return Ok(data);
            }
            else
            {
                return BadRequest();
            }
        }
    }

}
