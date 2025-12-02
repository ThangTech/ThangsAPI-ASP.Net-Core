# Entity Framework Core
---

## Giới thiệu

Entity Framework Core (EF Core) là một ORM giúp làm việc với database bằng cách sử dụng các đối tượng C#.

### Code First (Khuyên dùng)
- Bắt đầu bằng cách định nghĩa các class entity
- EF Core sẽ tự tạo database schema dựa trên các class này
- Phù hợp cho dự án mới hoặc khi bạn kiểm soát được database

### Database First
- Bắt đầu với database có sẵn
- EF Core sẽ tạo các class entity dựa trên schema của database
- Phù hợp khi làm việc với database cũ

---

## DbContext

**DbContext** là trung tâm tương tác với database trong EF Core, đóng vai trò như một phiên làm việc với database.

### Chức năng chính:
- Thiết lập kết nối với database
- Quản lý các entity (thêm, xóa, cập nhật)
- Truy vấn dữ liệu bằng LINQ
- Theo dõi thay đổi và lưu lại bằng `SaveChanges()`

---

## DbSet

**DbSet<TEntity>** đại diện cho một tập hợp các entity trong DbContext, tương ứng với một bảng trong database.

- Cung cấp các phương thức để truy vấn, thêm, cập nhật, xóa dữ liệu
- Ví dụ: `context.Persons.Add(newPerson);`

---

## Kết nối Database

**Connection string** là thông tin để ứng dụng kết nối tới database.

### Ví dụ connection string cho SQL Server:
```
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=PersonsDatabase;
Integrated Security=True;
Connect Timeout=30;
Encrypt=False;
```

### Nơi lưu trữ connection string:
- `appsettings.json` (khuyên dùng)
- Biến môi trường (cho production)
- User Secrets (cho development)

### Sử dụng connection string trong EF Core:
```csharp
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
```

---

## Seed Data

**Seed Data** là dữ liệu mẫu được thêm vào database khi tạo mới hoặc áp dụng migration.

### Mục đích:
- Tạo dữ liệu mẫu cho phát triển/test
- Dữ liệu tra cứu (quốc gia, danh mục...)
- Thiết lập giá trị mặc định

### Cách sử dụng:
- Định nghĩa seed data trong `OnModelCreating` của DbContext
- Sử dụng `HasData` để liên kết seed data với entity

---

## Migrations

**Migrations** giúp quản lý thay đổi cấu trúc database khi ứng dụng phát triển.

### Các lệnh migration phổ biến:
```powershell
Add-Migration <TenMigration>      # Tạo migration mới
Update-Database                  # Áp dụng migration vào database
Remove-Migration                 # Xóa migration cuối cùng (chưa áp dụng)
```

### Best Practices:
- Tạo migration nhỏ, tên rõ ràng
- Luôn backup database trước khi áp dụng migration
- Kiểm tra code migration trước khi thực hiện

---

## Fluent API

**Fluent API** là cách cấu hình mapping cho entity bằng code thay vì Data Annotations.

### Ví dụ cấu hình:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Person>()
        .ToTable("Persons")
        .HasKey(p => p.PersonID);
    modelBuilder.Entity<Person>()
        .Property(p => p.TIN)
        .HasColumnName("TaxIdentificationNumber")
        .HasColumnType("varchar(8)")
        .HasDefaultValue("ABC12345")
        .IsRequired();
    modelBuilder.Entity<Person>()
        .HasOne(p => p.Country)
        .WithMany(c => c.Persons)
        .HasForeignKey(p => p.CountryID);
}
```

---

## Relationships (Quan hệ giữa các bảng)

- **Primary Key**: Định danh duy nhất cho mỗi bản ghi
- **Foreign Key**: Tham chiếu đến primary key của bảng khác
- **Navigation Properties**: Điều hướng giữa các entity

### Ví dụ cấu hình quan hệ:
```csharp
modelBuilder.Entity<Person>()
    .HasOne(p => p.Country)
    .WithMany(c => c.Persons)
    .HasForeignKey(p => p.CountryID);
```

---

## CRUD với EF Core

### Thêm mới
```csharp
_db.Persons.Add(newPerson);
await _db.SaveChangesAsync();
```

### Đọc dữ liệu
```csharp
var persons = await _db.Persons.ToListAsync();
```

### Cập nhật
```csharp
var person = await _db.Persons.FindAsync(id);
person.PersonName = "Tên mới";
await _db.SaveChangesAsync();
```

### Xóa
```csharp
var person = await _db.Persons.FindAsync(id);
_db.Persons.Remove(person);
await _db.SaveChangesAsync();
```

---

## Tóm tắt các điểm quan trọng

- **DbContext**: Quản lý session với database
- **DbSet<T>**: Đại diện cho bảng
- **Entities**: Class C# đại diện cho bảng
- **LINQ**: Truy vấn dữ liệu
- **Migrations**: Quản lý thay đổi schema
- **Fluent API**: Cấu hình mapping bằng code
- **Relationships**: Cấu hình quan hệ giữa các bảng

---

## Câu hỏi phỏng vấn thường gặp

**EF Core là gì?**
> Là ORM giúp làm việc với database bằng đối tượng C#.

**Code First vs Database First?**
> Code First bắt đầu từ class, Database First bắt đầu từ database.

**Migrations là gì?**
> Quản lý thay đổi schema database.

**Navigation Properties là gì?**
> Property trong entity để điều hướng đến entity liên quan.

**Include() dùng để làm gì?**
> Eager loading dữ liệu liên quan, tránh N+1 query.

**Làm thế nào để tránh SQL injection?**
> Luôn dùng parameterized queries hoặc LINQ.

---
