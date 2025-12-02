# Entity Framework Core
### Code First (Khuyên dùng)
- Bắt đầu bằng cách định nghĩa entity classes
- EF Core tạo database schema dựa trên các class
- Phù hợp khi: Dự án mới hoặc có toàn quyền kiểm soát database
## Giới thiệu
### Database First
- Bắt đầu với database có sẵn
- EF Core tạo entity classes dựa trên schema
- Phù hợp khi: Làm việc với legacy database


- **Entities và DbContext**: Bạn định nghĩa các class đại diện cho các bảng trong database (entities) và class DbContext làm cầu nối giữa entities và database
- **Querying và Saving**: Sử dụng LINQ để truy vấn dữ liệu. EF Core chuyển đổi LINQ thành SQL và thực thi chúng

---
## Ưu điểm
## DbContext

**DbContext** là trung tâm tương tác với database trong EF Core. Nó đóng vai trò như một session với database.
- **Tăng năng suất**: Giảm code lặp lại cho các tương tác database
### Chức năng chính:
- **Type Safety**: LINQ cung cấp type safety tại compile-time
- **Kết nối Database**: Thiết lập kết nối sử dụng connection string
- **Quản lý Entities**: Theo dõi thay đổi, quản lý lifecycle (add, delete, update)
- **Querying Data**: Sử dụng LINQ để truy vấn, EF Core chuyển đổi thành SQL
- **Change Tracking**: Tự động theo dõi thay đổi, khi gọi `SaveChanges()` sẽ tạo SQL commands tương ứng
- **Change Tracking**: Tự động theo dõi thay đổi trên entities
## DbSet

**DbSet<TEntity>** đại diện cho một collection của entity type trong DbContext.

### Đặc điểm:
- **Learning Curve**: Cần thời gian để hiểu các khái niệm và best practices
- Mỗi DbSet được map với một bảng trong database
- Cung cấp các method để query, add, update, delete entities
- Ví dụ: `context.Persons.Add(newPerson);`

### Code Example
## Các NuGet Packages cần thiết
```csharp
You start by defining your entity classes, and EF Core creates the database schema based on those classes.

It's flexible and well-suited for rapid development.

Database First:

    public class PersonsDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }
It can save time initially but may require manual adjustments to the generated code.
        public PersonsDbContext(DbContextOptions options) : base(options) { }

Model First (Not in EF Core):

This approach involves designing a visual model (EDMX) of your database schema, and EF generates the code from the model.

While it was available in earlier versions of Entity Framework, it's not supported in EF Core.



Notes
```

### Lưu ý quan trọng:
        // OnModelCreating is used to customize your model mappings, but for this example we won't change anything.
- **Entity Classes**: Định nghĩa cấu trúc dữ liệu phù hợp với domain model
- **Data Annotations và Fluent API**: Dùng để cấu hình mapping
- **Relationships**: EF Core hỗ trợ one-to-one, one-to-many, many-to-many
- **Connection String**: Cần cấu hình đúng trong `appsettings.json`
- **Dependency Injection**: DbContext thường được inject như scoped service
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>().ToTable("Persons");
    }

---
In this example:
## Connection Strings
PersonsDbContext derives from DbContext.
**Connection string** là "địa chỉ" mà ứng dụng sử dụng để kết nối với database server.
Countries and Persons are DbSet properties representing the Country and Person entities respectively.
### Thông tin trong Connection String:
OnModelCreating is overridden to customize the mapping between your entities and database tables (though we aren't customizing anything in this case).
- **Data Source (Server)**: Tên hoặc IP của database server
- **Initial Catalog (Database Name)**: Tên database cụ thể
- **Credentials**: Username và password (nếu cần)
- **Additional Settings**: Timeout, encryption, v.v.

### Ví dụ SQL Server Connection String:

```
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=PersonsDatabase;
Integrated Security=True;
Connect Timeout=30;
Encrypt=False;
```

### Nơi lưu trữ Connection String
Notes
#### 1. appsettings.json (Khuyên dùng)

```json
DbSet:
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=...;Initial Catalog=...;"
  }
Provides methods for querying, adding, updating, and deleting entities.
```
Connection String: Ensure you provide the correct connection string in your appsettings.json (or environment variables) so EF Core knows how to connect to your database.
#### 2. Environment Variables (Cho Production)

```cmd
set ASPNETCORE_ConnectionStrings__DefaultConnection="..."
```

#### 3. User Secrets (Cho Development)

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "..."
```

### Sử dụng Connection String trong EF Core

```csharp
In the world of databases, a connection string is essentially the address your application uses to locate and connect to your database server. It contains vital information like:

    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );

```
Initial Catalog=PersonsDatabase: Indicates that the database to connect to is named "PersonsDatabase."
### Best Practices:
Integrated Security=True: Uses Windows authentication (the application's identity) to connect to the database.
- Dùng Environment Variables cho production để bảo mật
- Dùng User Secrets cho development
- Tách connection string cho từng môi trường (Development, Staging, Production)
- Cân nhắc dùng Azure Key Vault cho production
Encrypt=False, TrustServerCertificate=False: Options related to encryption and certificate validation (can be set to true for production environments).

ApplicationIntent=ReadWrite: Specifies the intended use of the connection (read/write in this case).

MultiSubnetFailover=False: Relates to high availability scenarios (not relevant for most basic setups).







Storing Connection Strings in ASP.NET Core
appsettings.json (Recommended):

The preferred location for storing your connection string (and other configuration settings).

It's organized by sections (like "ConnectionStrings"):

{
"ConnectionStrings": {
"DefaultConnection": "..." // Your connection string here
}
}


Environment Variables:

More secure for sensitive information, as environment variables are not stored in code.

Use the prefix ConnectionStrings__ for your connection string environment variable:

Bash

set ASPNETCORE_ConnectionStrings__DefaultConnection="..."  // In Command Prompt
$env:ASPNETCORE_ConnectionStrings__DefaultConnection = "..." // In PowerShell


User Secrets (Development Only):

Best for keeping sensitive information out of your source code during development.

Use the dotnet user-secrets commands to manage them.



Injecting and Using the Connection String in EF Core

// Program.cs
builder.Services.AddDbContext<PersonsDbContext>(options => {
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
AddDbContext<PersonsDbContext>(): Registers your DbContext with the DI container and configures it.

options.UseSqlServer(...): Specifies that you're using SQL Server and provides the connection string.

builder.Configuration.GetConnectionString("DefaultConnection"): Retrieves the connection string from the "ConnectionStrings:DefaultConnection" key in the configuration.



Best Practices

Separate Environments: Store different connection strings for development, staging, and production environments (e.g., appsettings.Development.json, appsettings.Production.json).

Environment Variables in Production: Use environment variables to store your production connection string for security.

User Secrets in Development: Use user secrets during development to keep sensitive data out of source control.

Secure Storage: Consider using Azure Key Vault or other secret management solutions for production environments.

Connection Resiliency: For production, implement connection resiliency strategies to handle transient database errors.





---

## Seed Data

**Seed Data** là dữ liệu khởi tạo ban đầu được đưa vào database khi tạo mới hoặc áp dụng migration.

### Mục đích:

- **Initial Data**: Tạo dữ liệu mẫu cho development/testing
- **Reference Data**: Dữ liệu tra cứu (quốc gia, danh mục, v.v.)
- **Default Values**: Thiết lập giá trị mặc định

### Cách hoạt động:

1. Định nghĩa seed data trong method `OnModelCreating` của DbContext
2. Sử dụng method `HasData` để liên kết seed data với entities
3. EF Core tự động tạo SQL statements khi áp dụng migration

### Code Example

```csharp
// PersonsDbContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Country>().ToTable("Countries");
    modelBuilder.Entity<Person>().ToTable("Persons");

    // Seed Data cho Countries
    string countriesJson = System.IO.File.ReadAllText("countries.json");
    List<Country> countries = System.Text.Json.JsonSerializer
        .Deserialize<List<Country>>(countriesJson);
    
    foreach (Country country in countries)
    {
        modelBuilder.Entity<Country>().HasData(country);
    }

    // Seed Data cho Persons
    string personsJson = System.IO.File.ReadAllText("persons.json");
    List<Person> persons = System.Text.Json.JsonSerializer
        .Deserialize<List<Person>>(personsJson);
    
    foreach (Person person in persons)
    {
        modelBuilder.Entity<Person>().HasData(person);
    }
}
```

### Best Practices:

- Tách seed data ra file riêng (JSON, CSV)
- Đảm bảo seed data có thể áp dụng nhiều lần mà không gây lỗi
- Chú ý thứ tự seeding để tránh lỗi foreign key
- Với dataset lớn, dùng bulk insert để tối ưu performance





---

## Code First Migrations

**Migrations** là cách quản lý thay đổi database schema theo thời gian khi ứng dụng phát triển.

### Mục đích:

- **Theo dõi thay đổi**: Tạo migration scripts đại diện cho những thay đổi trong entity classes
- **Version Control**: Mỗi migration có version riêng, có thể track trong source control
- **Áp dụng thay đổi**: Sử dụng `dotnet ef` hoặc Package Manager Console để cập nhật database
- **Rollback**: Có thể hoàn tác các thay đổi nếu cần

### Khi nào sử dụng:

- Theo phương pháp Code First
- Khi thay đổi entity classes (thêm property, đổi tên table, sửa relationships)
- Làm việc nhóm để đồng bộ database

### Các lệnh Migration (Package Manager Console):

```powershell
# Tạo migration mới
Add-Migration <TenMigration>

# Xem danh sách migrations
Get-Migrations

# Cập nhật database
Update-Database

# Rollback về migration cụ thể
Update-Database -TargetMigration <TenMigration>

# Xóa migration cuối cùng (chưa apply)
Remove-Migration
```

### Ví dụ quy trình:

```powershell
# 1. Thêm migration
Add-Migration "AddProductTable"

# 2. Xem migration được tạo trong folder Migrations

# 3. Áp dụng vào database
Update-Database
```

### Best Practices:

- **Migrations nhỏ**: Tạo migrations nhỏ, tập trung vào một tính năng
- **Tên rõ ràng**: Đặt tên mô tả (VD: "AddProductTable", "RenameCustomerColumn")
- **Bảo tồn dữ liệu**: Thiết kế migrations tránh mất dữ liệu
- **Review code**: Luôn kiểm tra migration code trước khi apply
- **Backup**: Luôn backup database trước khi apply migrations ở production





---

## Fluent API

**Fluent API** là cách thay thế cho Data Annotations để cấu hình domain model trong code.

### Tại sao dùng Fluent API?

- **Linh hoạt**: Nhiều tùy chọn cấu hình hơn Data Annotations
- **Tách biệt mối quan tâm**: Giữ entity classes sạch, không bị rối với database attributes
- **Dễ đọc**: Cú pháp method chaining dễ hiểu

### Cấu hình trong OnModelCreating

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Cấu hình entity và table
    modelBuilder.Entity<Person>()
        .ToTable("Persons")
        .HasKey(p => p.PersonID);

    // Cấu hình property
    modelBuilder.Entity<Person>()
        .Property(p => p.TIN)
        .HasColumnName("TaxIdentificationNumber")
        .HasColumnType("varchar(8)")
        .HasDefaultValue("ABC12345")
        .IsRequired();

    // Cấu hình relationship
    modelBuilder.Entity<Person>()
        .HasOne(p => p.Country)
        .WithMany(c => c.Persons)
        .HasForeignKey(p => p.CountryID);

    // Tạo index
    modelBuilder.Entity<Person>()
        .HasIndex(p => p.Email)
        .IsUnique();

    // Thêm check constraint
    modelBuilder.Entity<Person>()
        .HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8");
}
```

### Các method quan trọng:

#### Entity-Level:
- `ToTable()`: Đặt tên table
- `HasKey()`: Chỉ định primary key
- `HasIndex()`: Tạo index
- `Ignore()`: Loại trừ property khỏi mapping

#### Property-Level:
- `HasColumnName()`: Đặt tên column
- `HasColumnType()`: Đặt kiểu dữ liệu
- `IsRequired()`: Bắt buộc (not null)
- `HasMaxLength()`: Độ dài tối đa
- `HasDefaultValue()`: Giá trị mặc định

#### Relationship:
- `HasOne()` / `WithMany()`: Quan hệ one-to-many
- `HasMany()` / `WithOne()`: Quan hệ many-to-one
- `HasForeignKey()`: Chỉ định foreign key





---

## Relationships (Quan hệ giữa các bảng)

### Các khái niệm quan trọng:

- **Referential Integrity**: Đảm bảo tính nhất quán giữa các bảng có quan hệ
- **Primary Key**: Định danh duy nhất cho mỗi record trong table
- **Foreign Key**: Cột tham chiếu đến primary key của table khác

### Cấu hình Relationships

#### Navigation Properties:

```csharp
// Country entity
public class Country
{
    public Guid CountryID { get; set; }
    public string CountryName { get; set; }
    
    // Navigation property
    public virtual ICollection<Person>? Persons { get; set; }
}

// Person entity
public class Person
{
    public Guid PersonID { get; set; }
    public string PersonName { get; set; }
    public Guid? CountryID { get; set; }
    
    // Navigation property
    public virtual Country? Country { get; set; }
}
```

#### Fluent API Configuration:

```csharp
modelBuilder.Entity<Person>(entity =>
{
    entity.HasOne(p => p.Country)
          .WithMany(c => c.Persons)
          .HasForeignKey(p => p.CountryID);
});
```

### LINQ Queries với Relationships

#### Query đơn giản:

```csharp
// Tìm tất cả persons từ USA
var peopleFromUSA = _dbContext.Persons
    .Where(p => p.Country.CountryName == "USA")
    .ToList();
```

#### Eager Loading với Include():

```csharp
// Load cả Country cùng Person (tránh N+1 query)
var peopleFromUSA = _dbContext.Persons
    .Include(p => p.Country)
    .Where(p => p.Country.CountryName == "USA")
    .ToList();
```

### Best Practices:

- Chọn đúng loại relationship (one-to-one, one-to-many, many-to-many)
- Cấu hình cascading behavior (xóa parent thì xử lý children như thế nào)
- Tránh lạm dụng `Include()` nếu không cần thiết
- Sử dụng explicit loading khi chỉ cần load related data trong một số trường hợp





---

## CRUD Operations với EF Core

### Các phương thức cơ bản:

#### Create (Thêm mới)
```csharp
_db.Persons.Add(newPerson);
await _db.SaveChangesAsync();
```

#### Read (Đọc dữ liệu)
```csharp
// Lấy tất cả
var persons = await _db.Persons.ToListAsync();

// Lấy theo điều kiện
var person = await _db.Persons
    .FirstOrDefaultAsync(p => p.PersonID == id);

// Lấy kèm related data
var persons = await _db.Persons
    .Include(p => p.Country)
    .ToListAsync();
```

#### Update (Cập nhật)
```csharp
var person = await _db.Persons.FindAsync(id);
person.PersonName = "New Name";
await _db.SaveChangesAsync();
```

#### Delete (Xóa)
```csharp
var person = await _db.Persons.FindAsync(id);
_db.Persons.Remove(person);
await _db.SaveChangesAsync();
```

### Best Practices:

- Sử dụng **Async operations** (`ToListAsync()`, `SaveChangesAsync()`) để tăng hiệu suất
- Sử dụng **Dependency Injection** cho DbContext
- Sử dụng **DTOs** để tách biệt domain model và presentation layer
- Implement cả **server-side** và **client-side validation**
- Xử lý exceptions đúng cách

---

## Stored Procedures (Tùy chọn)

### Thực thi Stored Procedure:

```csharp
// Query trả về dữ liệu
public List<Person> sp_GetAllPersons()
{
    return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
}

// Command không trả về dữ liệu
public int sp_InsertPerson(Person person)
{
    SqlParameter[] parameters = new SqlParameter[] 
    {
        new SqlParameter("@PersonID", person.PersonID),
        new SqlParameter("@PersonName", person.PersonName),
        // ... other parameters
    };
    
    return Database.ExecuteSqlRaw(
        "EXECUTE [dbo].[InsertPerson] @PersonID, @PersonName, ...", 
        parameters);
}
```

### Lưu ý:

- Luôn sử dụng **parameterized queries** để tránh SQL injection
- Stored procedures phù hợp cho logic phức tạp hoặc tối ưu performance

---

## Tóm tắt các điểm quan trọng

### Khái niệm cốt lõi:

1. **DbContext**: Session với database, theo dõi thay đổi
2. **DbSet<T>**: Collection của entity type
3. **Entities**: C# classes đại diện cho database tables
4. **LINQ**: Query dữ liệu bằng C#
5. **Migrations**: Quản lý thay đổi database schema

### Code First Migrations:

```powershell
Add-Migration "TenMigration"    # Tạo migration
Update-Database                  # Áp dụng migration
Remove-Migration                 # Xóa migration cuối
```

### Fluent API:

- Cấu hình entity: `ToTable()`, `HasKey()`, `HasIndex()`
- Cấu hình property: `HasColumnName()`, `HasColumnType()`, `IsRequired()`
- Cấu hình relationship: `HasOne()`, `WithMany()`, `HasForeignKey()`

### Relationships:

- **One-to-Many**: Country → Persons
- **Navigation Properties**: Điều hướng giữa các entities
- **Include()**: Eager loading related data
- **Foreign Keys**: Đảm bảo referential integrity

### Best Practices:

✅ Sử dụng Async operations
✅ Implement Repository Pattern
✅ Sử dụng DTOs
✅ Connection Resiliency cho production
✅ Tách connection string theo môi trường
✅ Migrations nhỏ và có tên rõ ràng
✅ Backup database trước khi apply migrations
✅ Review migration code trước khi apply

### Các lỗi thường gặp:

❌ Không sử dụng `Include()` → N+1 query problem
❌ Lạm dụng `Include()` → Fetch quá nhiều data
❌ Không parameterize SQL queries → SQL injection
❌ Không backup trước khi apply migrations
❌ Migrations quá lớn → Khó rollback

---

## Câu hỏi phỏng vấn thường gặp

**Q: EF Core là gì?**
A: Là ORM framework giúp đơn giản hóa tương tác với database bằng cách cho phép làm việc với dữ liệu như .NET objects.

**Q: Code First vs Database First?**
A: Code First bắt đầu từ C# classes, Database First bắt đầu từ database có sẵn.

**Q: Migrations là gì?**
A: Là cách quản lý thay đổi database schema theo thời gian, có thể version control và rollback.

**Q: Navigation Properties là gì?**
A: Là properties trong entity classes cho phép điều hướng đến related entities.

**Q: Include() dùng để làm gì?**
A: Eager loading related entities trong một query duy nhất, tránh N+1 query problem.

**Q: Làm thế nào để tránh SQL injection?**
A: Luôn sử dụng parameterized queries hoặc LINQ thay vì string concatenation.

