using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThangAPI.Models.Domain;
using ThangAPI.Models.DTO;
using ThangAPI.Repositoty;

namespace ThangAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WallkController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WallkController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkDTO addWalkDTO)
        {
            // chuyen data tu dto sang domain model
            var walkDomain = mapper.Map<Walkcs>(addWalkDTO);

            await walkRepository.CreateWalkAsync(walkDomain);

            // chuyen tu domain sang DTO
            var walkDTO = mapper.Map<WalkDTO>(walkDomain);
            return CreatedAtAction(nameof(GetByID), new {id = walkDTO.Id}, walkDTO);
            
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // chuyen tu domain sang dto
            var walkDomain = await walkRepository.GetAllWalkAsync();
            
            return Ok(mapper.Map<List<WalkDTO>>(walkDomain)); // Trả về một list

        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByID([FromRoute] Guid id)
        {
            var walkDomain = await walkRepository.GetWalkIDAsync(id);
            if(walkDomain == null)
            {
                return NotFound();
            }
            // chuyen domain sang dto
            var walkDTO = mapper.Map<WalkDTO>(walkDomain);
            return Ok(walkDTO);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkDTO updateWalkDTO) // From Body
        {
            // tu DTO sang Model
            var walkDomain = mapper.Map<Walkcs>(updateWalkDTO);
            if(walkDomain == null)
            {
                return NotFound();
            }
            walkDomain = await walkRepository.UpdateWalkAsync(id, walkDomain);

            // chuyen tu domain sang DTO
            var walkDTO = mapper.Map<WalkDTO>(walkDomain);
            return Ok(walkDTO);

        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var walkDomain =  await walkRepository.DeleteWalkAsync(id);
            if( walkDomain == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<WalkDTO>(walkDomain);
            return Ok(walkDTO);
        }
    }
}
