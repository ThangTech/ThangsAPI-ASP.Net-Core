
## Các bước cơ bản để sử dụng FluentValidation trong .NET

1. Cài đặt gói NuGet:
   - Mở Package Manager Console và chạy lệnh:
     ```
     Install-Package FluentValidation
     ```

2. Tạo lớp Validator:
   - Tạo một lớp kế thừa từ `AbstractValidator<T>` trong đó `T` là kiểu dữ liệu bạn muốn xác thực.
   - Định nghĩa các quy tắc xác thực trong constructor của lớp này.
   
   ```csharp
   using FluentValidation;

   public class CustomValidator : AbstractValidator<Todo>
   {
       public CustomValidator()
       {
           RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty!!!");
           RuleFor(x => x.Name).MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
           RuleFor(x => x.IsComplete).NotNull().WithMessage("IsComplete must be specified");
       }
   }
   ```

3. Sử dụng Validator:
   - Tạo một instance của lớp Validator và gọi phương thức `Validate` để kiểm tra dữ liệu.
   
   ```csharp
   app.MapPost("/todoitems", async (Todo todo, TodoDb db, IValidator<Todo> validator) =>
   {
       var validationResult = await validator.ValidateAsync(todo);
       if (!validationResult.IsValid)
       {
           return Results.ValidationProblem(validationResult.ToDictionary());
       }
       await db.Todos.AddAsync(todo);
       await db.SaveChangesAsync();

       return Results.Created($"/todoitems/{todo.Id}", todo);
   });
   ```

4. Tích hợp với ASP.NET Core (nếu cần):
   - Đăng ký FluentValidation trong `Startup.cs` hoặc `Program.cs`.
   
   ```csharp
   builder.Services.AddScoped<IValidator<Todo>, CustomValidator>();
   ```
