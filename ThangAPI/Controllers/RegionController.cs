using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ThangAPI.CustomActionFilters;
using ThangAPI.Data;
using ThangAPI.Models.Domain;
using ThangAPI.Models.DTO;
using ThangAPI.Repositoty;


namespace ThangAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        //private readonly ThangDbContext thangDbContext; //Depenjency
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionController> logger;

        public RegionController(IRegionRepository regionRepository, IMapper mapper, ILogger<RegionController> logger)
        {
            //this.thangDbContext = thangDbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        //public IActionResult GetAll()
        //{
        //    var regions = new List<Region>
        //    {
        //        new Region
        //        {
        //            Id = Guid.NewGuid(),
        //            Name = "Manchester",
        //            Code = "MC",
        //            RegionImageURL = ""
        //        },
        //        new Region
        //        {
        //            Id = Guid.NewGuid(),
        //            Name = "London",
        //            Code = "LON",
        //            RegionImageURL = ""
        //        }
        //    };
        //    return Ok(regions);
        //}
        public async Task<IActionResult> GetALl()
        {
            logger.LogInformation("GetAllRegions");
            // lay du lieu tu database - domain models
            var regions = await regionRepository.GetAllAsync();

            // dua du lieu tu domain models toi DTOs
            //var regionDTO = new List<RegionDTO>();
            //foreach(var region in regions)
            //{
            //    regionDTO.Add(new RegionDTO
            //    {
            //        Id = region.Id,
            //        Name = region.Name,
            //        Code = region.Code,
            //        RegionImageURL = region.RegionImageURL
            //    }
            //    );
            //}
            logger.LogInformation($"Finish GetAllRgions: {JsonSerializer.Serialize(regions)}");
            // dua du lieu tu domain models toi DTOs bằng mapper
            var regionDTO = mapper.Map<List<RegionDTO>>(regions);
            // tra du lieu bang DTOS
            return Ok(regionDTO);
        }
        // Lay region theo id
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetByID([FromRoute] Guid id)
        {
            //var region = thangDbContext.Regions.Find(id);
            // lay du lieu tu database - domain models
            var regionDomain = await regionRepository.GetByIDAsync(id);
            if(regionDomain == null)
            {
                return NotFound();
            }
            // dua du lieu tu domain models toi DTO
            //var regionDTO = new RegionDTO
            //{
            //    Id = regionDomain.Id,
            //    Name = regionDomain.Name,
            //    Code = regionDomain.Code,
            //    RegionImageURL = regionDomain.RegionImageURL
            //};
            var regionDTO = mapper.Map<RegionDTO>(regionDomain);
            // tra ve DTO
            return Ok(regionDTO);
        }
        // POST
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionDTO addRegionDTO)
        {
            // Chuyen du lieu tu DTO den Domain Models
            //var regionDomainModel = new Region
            //{
            //    Code = addRegionDTO.Code,
            //    Name = addRegionDTO.Name,
            //    RegionImageURL= addRegionDTO.RegionImageURL
            //};
           
                var regionDomainModel = mapper.Map<Region>(addRegionDTO);
                // Dung domain models de tao Region
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                // Chuyen domain models sang DTO 
                //var regionDTO = new RegionDTO
                //{
                //    Id = regionDomainModel.Id,
                //    Code = regionDomainModel.Code,
                //    Name = regionDomainModel.Name,
                //    RegionImageURL = regionDomainModel.RegionImageURL
                //};
                var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);
                return CreatedAtAction(nameof(GetByID), new { id = regionDTO.Id }, regionDTO);
            
        }
        // Update
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDTO updateRegionDTO)
        {
            //chuyen du lieu tu DTO den model
            //var regionDomainModel = new Region
            //{
            //    Code = updateRegionDTO.Code,
            //    Name = updateRegionDTO.Name,
            //    RegionImageURL = updateRegionDTO.RegionImageURL
                //};
                var regionDomainModel = mapper.Map<Region>(updateRegionDTO);
                // Kiem tra su ton tai cua Region
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }
                // chuyen du lieu tu domain den DTO
                //var regionDTO = new RegionDTO
                //{
                //    Id = regionDomainModel.Id,
                //    Code = regionDomainModel.Code,
                //    Name = regionDomainModel.Name,
                //    RegionImageURL = regionDomainModel.RegionImageURL
                //};
                var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);
                return Ok(regionDTO);
        }
        // Delete
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer, Reader")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            // chuyen tu domain model sang DTO
            //var regionDTO = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageURL = regionDomainModel.RegionImageURL
            //};
            var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);
            return Ok(regionDTO);

        }

    }
}
