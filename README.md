# 1. 🎓 Student API – API đầu tiên bằng ASP.NET Core

Đây là chương trình API đầu tiên của tôi, sử dụng **C# với ASP.NET Core Web API**. Mục tiêu của API này là cung cấp danh sách sinh viên thông qua một endpoint đơn giản.
## Mô tả
API cung cấp danh sách sinh viên mẫu khi gửi yêu cầu `GET` đến endpoint `/api/student`.
## Cấu trúc Controller

```csharp
namespace ThangAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudent()
        {
            string[] studentNames = new string[]
            {
                "Dan",
                "Thang",
                "Son",
                "Phuc",
                "Thuan"
            };
            return Ok(studentNames);
        }
    }
}
```
## Cấu hình `appsetting.json` để kết nối đến Database
```csharp
{
  "ConnectionStrings": {
    "ThangConnectionString": "Server=YourServer; Database=ThangAPI; Trusted_Connection=True; TrustServerCertificate=True"
  }
}
```
## Trong `Program.cs` thiết lập cấu hình kết nối
```csharp
builder.Services.AddDbContext<ThangDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("ThangConnectionString")));
```
## Tạo thư mục `Model` 
Bên trong tạo thư mục `Domain` lưu thông tin. 
```csharp
﻿namespace ThangAPI.Models.Domain
{
    public class Difficulty
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
```
## Thêm DbContext và sử dụng Entity Framework Core

Tạo folder `Data` thêm lớp DbContext làm việc với Database. Tạo bảng với 3 cột.

### 🔹 Mã nguồn `ThangDbContext.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using ThangAPI.Models.Domain;

namespace ThangAPI.Data
{
    public class ThangDbContext : DbContext
    {
        public ThangDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walkcs> Walkcs { get; set; }
    }
}
```
## Mở cửa sổ Package Thêm `Migration` làm việc với Database
```
# 1. Cài đặt gói hỗ trợ Migration (chạy 1 lần nếu chưa cài)
dotnet add package Microsoft.EntityFrameworkCore.Design

# 2. Tạo Migration đầu tiên
dotnet ef migrations add InitialCreate

# 3. Áp dụng Migration vào SQL Server
dotnet ef database updateg
```
# 2. Làm việc với `Database` thông qua `DTOs`
## Tạo thư mục `DTOs` bên trong thư mục `Model` lấy các property từ class.
```csharp
﻿namespace ThangAPI.Models.DTO
{
    public class RegionDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageURL { get; set; }
    }
}
```
## CRUD với `DTOs`
Lấy các property muốn tương tác trong `DTOs`
```csharp
﻿namespace ThangAPI.Models.DTO
{
    public class UpdateRegionDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageURL { get; set; }
    }
}
```
## Sử dụng DTOs trong `Controller`
```csharp
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
```
# 3. `Repository`, `Async/ await` và `AutoMapper`
## Triển khai project một cách an toàn và hiệu quả
Việc kết nối đến `Database` trong `Controller` là một việc không nên làm. Hợp lý hơn hãy triển khai nó trong `Repository`. Nó sẽ giúp code của bạn dễ bảo trì và hiệu quả hơn.
## Tạo một folder `Repository`  tạo interface `IRegionRepository`
Bên trong tạo các phương thức để triển khai.
```csharp
using ThangAPI.Models.Domain;

namespace ThangAPI.Repositoty
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?>GetByIDAsync(Guid id);
        Task<Region?> CreateAsync(Region region);

        Task<Region?>UpdateAsync(Guid id, Region region);

        Task<Region>DeleteAsync(Guid id);
    }
}
```
Tạo thêm một class để kết nối `Database` và xử lý logic trong đây
```csharp
using Microsoft.EntityFrameworkCore;
using ThangAPI.Data;
using ThangAPI.Models.Domain;

namespace ThangAPI.Repositoty
{
    public class SQLRegionRepositorycs:IRegionRepository
    {
        private readonly ThangDbContext thangDbContext;

        public SQLRegionRepositorycs(ThangDbContext thangDbContext)
        {
            this.thangDbContext = thangDbContext;
        }

        public async Task<Region?> CreateAsync(Region region)
        {
           await thangDbContext.Regions.AddAsync(region);
           await thangDbContext.SaveChangesAsync();
           return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var existRegion = thangDbContext.Regions.FirstOrDefault(x => x.Id == id);  
            if (existRegion == null)
            {
                return null;
            }
            thangDbContext.Remove(existRegion);
            await thangDbContext.SaveChangesAsync();
            return existRegion;

        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await thangDbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIDAsync(Guid id)
        {
            return await thangDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existRegion = await thangDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existRegion == null)
            {
                return null;
            }
            existRegion.Code = region.Code;
            existRegion.Name = region.Name;
            existRegion.RegionImageURL = region.RegionImageURL;
            await thangDbContext.SaveChangesAsync();
            return existRegion;
        }
    }
}
Cấu hình trong Program.cs
builder.Services.AddScoped<IRegionRepository, SQLRegionRepositorycs>(); //Repository Pattern
```
## Xử lý bất đồng bộ với Async/ await
Bạn thấy các dòng code trên có gì đặc biệt không. Chúng ta có các từ khóa như `Async/ await` và `Task<>` vậy nó là gì???
Đôi khi không phải lúc nào việc trả dữ liệu từ Database diễn ra một cách suôn sẻ. Có thể có nhiều vấn đề xảy ra khi lấy dữ liệu, sử dụng async/ await và Task<>.
1. Có thể xử lý được nhiều request hơn cùng lúc, vì không bị chiếm giữ tài nguyên khi chờ dữ liệu
2. Ứng dụng không bị chậm tổng thể dù đang chờ một thao tác tốn thời gian
3. Hệ thống không bị đứng yên để chờ database trả kết quả...vv
```csharp
public async Task<Region?> CreateAsync(Region region)
        {
           await thangDbContext.Regions.AddAsync(region);
           await thangDbContext.SaveChangesAsync();
           return region;
        }

//Phương thức Remove sẽ không có Async
```
## Sức mạnh của `AutoMapper`
```csharp
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
```
Bạn thấy code trở nên vô cùng phức tạp khi bạn phải ánh xạ dữ liệu từ `DTOs` vào `Domain` rồi ngược lại.
AutoMapper sẽ giúp bạn tự động ánh xạ.
```csharp
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>()); // Add automapper
// Cấu hình AutoMapper
```
Tạo thư muc `Mapping` thêm class để tạo ánh xạ.
```csharp
using AutoMapper;
using ThangAPI.Models.Domain;
using ThangAPI.Models.DTO;

namespace ThangAPI.Mapping
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<AddRegionDTO, Region>().ReverseMap();
            CreateMap<UpdateRegionDTO, Region>().ReverseMap();
        }
    }
}
```
## Validation 
```csharp

namespace ThangAPI.Models.DTO
{
    public class AddRegionDTO
    {
        [Required] // Validation
        [MinLength(3, ErrorMessage = "Code have to minimum 3 of character")]
        [MaxLength(3, ErrorMessage = "Code have to maximum 3 of character")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? RegionImageURL { get; set; }
    }
```
Tạo thư mục `CustomActionFilters` để xử validation một cách hiệu quả.
```csharp
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ThangAPI.CustomActionFilters
{
    public class ValidateModelAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.ModelState.IsValid == false)
            {
                context.Result = new BadRequestResult();
            }
        }
    }
}
```





