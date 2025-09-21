using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThangAPI.Data;
using ThangAPI.Models.Domain;
using ThangAPI.Models.DTO;


namespace ThangAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly ThangDbContext thangDbContext;

        public RegionController(ThangDbContext thangDbContext)
        {
            this.thangDbContext = thangDbContext;
        }
        [HttpGet]
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
        public IActionResult GetALl()
        {
            // lay du lieu tu database - domain models
            var regions = thangDbContext.Regions.ToList();

            // dua du lieu tu domain models toi DTOs
            var regionDTO = new List<RegionDTO>();
            foreach(var region in regions)
            {
                regionDTO.Add(new RegionDTO
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageURL = region.RegionImageURL
                }
                );
            }
            // tra du lieu bang DTOS
            return Ok(regionDTO);
        }
        // Lay region theo id
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetByID([FromRoute] Guid id)
        {
            //var region = thangDbContext.Regions.Find(id);
            // lay du lieu tu database - domain models
            var regionDomain = thangDbContext.Regions.FirstOrDefault(x => x.Id == id);
            if(regionDomain == null)
            {
                return NotFound();
            }
            // dua du lieu tu domain models toi DTO
            var regionDTO = new RegionDTO
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageURL = regionDomain.RegionImageURL
            };
            // tra ve DTO
            return Ok(regionDTO);
        }
        // POST
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionDTO addRegionDTO)
        {
            // Chuyen du lieu tu DTO den Domain Models
            var regionDomainModel = new Region
            {
                Code = addRegionDTO.Code,
                Name = addRegionDTO.Name,
                RegionImageURL= addRegionDTO.RegionImageURL
            };
            // Dung domain models de tao Region
            thangDbContext.Regions.Add(regionDomainModel);
            thangDbContext.SaveChanges();

            // Chuyen domain models sang DTO
            var regionDTO = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageURL = regionDomainModel.RegionImageURL
            };
            return CreatedAtAction(nameof(GetByID), new {id = regionDTO.Id}, regionDTO);
        }
        // Update
        [HttpPut]
        [Route("{id:Guid}")]

        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionDTO updateRegionDTO)
        {
            // Kiem tra su ton tai cua Region
            var regionDomainModel = thangDbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            // Dua du lieu tu DTO den domain models
            regionDomainModel.Code = updateRegionDTO.Code;
            regionDomainModel.Name = updateRegionDTO.Name; 
            regionDomainModel.RegionImageURL = updateRegionDTO.RegionImageURL;
            thangDbContext.SaveChanges();
            // chuyen du lieu tu domain den DTO
            var regionDTO = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageURL = regionDomainModel.RegionImageURL
            };
            return Ok(regionDTO);
        }
        // Delete
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomainModel = thangDbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            // Delete
            thangDbContext.Regions.Remove(regionDomainModel);
            thangDbContext.SaveChanges();
            // tra ve region da xoa
            // chuyen tu domain model sang DTO
            var regionDTO = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageURL = regionDomainModel.RegionImageURL
            };
            return Ok(regionDTO);

        }

    }
}
