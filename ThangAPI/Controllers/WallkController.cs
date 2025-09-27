using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThangAPI.CustomActionFilters;
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
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkDTO addWalkDTO)
        {
           
                // chuyen data tu dto sang domain model
                var walkDomain = mapper.Map<Walkcs>(addWalkDTO);

                await walkRepository.CreateWalkAsync(walkDomain);

                // chuyen tu domain sang DTO
                var walkDTO = mapper.Map<WalkDTO>(walkDomain);
                return CreatedAtAction(nameof(GetByID), new { id = walkDTO.Id }, walkDTO);
           
            
        }
        // api/walkcs/filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        //FilterOn lọc trên cột nào
        //FilterQuery câu truy vấn
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy,[FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            // chuyen tu domain sang dto
            var walkDomain = await walkRepository.GetAllWalkAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
            // Toán tử ?? nếu phép toán bên trái null hoặc rỗng thì lấy giá trị bên phải
            
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
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkDTO updateWalkDTO) // From Body
        {
                // tu DTO sang Model
                var walkDomain = mapper.Map<Walkcs>(updateWalkDTO);
                if (walkDomain == null)
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
