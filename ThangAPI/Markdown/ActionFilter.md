# Action Filters trong ASP.NET Core
## Khái niệm

Action Filter là một thành phần trong ASP.NET Core MVC pipeline cho phép bạn can thiệp vào quá trình thực thi của action method trong controller — cả trước khi action chạy (Before Execution) và sau khi action chạy (After Execution).

**Khác với Middleware:**

- Middleware áp dụng cho mọi request (toàn ứng dụng).
- Action Filter chỉ áp dụng cho các controller hoặc action cụ thể.

## Quy trình xử lý Request (Pipeline Flow)

Khi người dùng gửi HTTP Request, luồng xử lý trong ASP.NET Core như sau:

1. Request đi vào Middleware pipeline (các tầng như: Authentication, Authorization, Logging, Routing, Exception Handling, …).
2. Sau khi qua các middleware và được định tuyến đến controller phù hợp, hệ thống sẽ:
   - Gọi các Filter liên quan (Action Filter, Authorization Filter, Result Filter,…).
   - Thực thi các Action trong controller.
3. Sau khi action hoàn tất, các Filter sau execution chạy để xử lý kết quả trước khi response gửi về client.

## Các loại Filter chính trong ASP.NET Core
| Loại Filter | Mục đích chính | Interface triển khai |
|-------------|----------------|----------------------|
| Authorization Filter | Xác thực quyền truy cập | IAuthorizationFilter, IAsyncAuthorizationFilter |
| Resource Filter | Chạy trước khi model binding | IResourceFilter, IAsyncResourceFilter |
| Action Filter | Trước & sau khi gọi action | IActionFilter, IAsyncActionFilter |
| Exception Filter | Bắt & xử lý exception | IExceptionFilter, IAsyncExceptionFilter |
| Result Filter | Trước & sau khi trả kết quả | IResultFilter, IAsyncResultFilter |

## Triển khai một Action Filter

Tạo một class kế thừa IActionFilter:

```csharp
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

public class SampleFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        Debug.WriteLine("Action bắt đầu chạy...");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        Debug.WriteLine("Action đã chạy xong!");
    }
}
```

## Cách đăng ký sử dụng Filter

### Dùng toàn cục (Global Filter)

Thêm filter trong `Program.cs` (hoặc `Startup.cs`):

```csharp
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new SampleFilter());
});
```

Filter này sẽ áp dụng cho tất cả controllers và actions.

### Dùng cho từng Controller hoặc Action

Áp dụng bằng Attribute:

```csharp
[SampleFilter]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
```

Chỉ áp dụng cho controller hoặc action được đánh dấu.

## Filter bất đồng bộ

Nếu cần hỗ trợ xử lý asynchronous (ví dụ: ghi log vào database hoặc API ngoài), bạn có thể dùng interface:

```csharp
public class SampleAsyncFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Trước khi action chạy
        Console.WriteLine("Before Execution");

        var resultContext = await next(); // gọi action

        // Sau khi action chạy
        Console.WriteLine("After Execution");
    }
}
```

## Tóm tắt so sánh

| Thành phần | Phạm vi | Thời điểm chạy | Mục đích |
|------------|---------|-----------------|----------|
| Middleware | Toàn ứng dụng | Trước khi vào MVC | Xử lý logic chung như logging, auth, exception |
| Action Filter | Controller/Action | Trước & sau Action | Can thiệp luồng action, validate, log, cache |
