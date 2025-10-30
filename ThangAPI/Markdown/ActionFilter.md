# Action Filters trong ASP.NET Core
## Khái niệm

**Action Filter** là một thành phần trong ASP.NET Core MVC pipeline cho phép bạn can thiệp vào quá trình thực thi của action method trong controller — cả **trước khi action chạy** (Before Execution) và **sau khi action chạy** (After Execution).

### Khác biệt với Middleware

| Thành phần | Phạm vi áp dụng |
|------------|----------------|
| **Middleware** | Áp dụng cho mọi request trong toàn ứng dụng |
| **Action Filter** | Áp dụng cho một controller hoặc action cụ thể |

---

## Quy trình xử lý Request

Khi người dùng gửi **HTTP Request**, luồng xử lý trong ASP.NET Core như sau:

1. Request đi qua **Middleware pipeline** (các tầng như: Authentication, Authorization, Logging, Routing, Exception Handling,...)

2. Sau khi định tuyến đến đúng controller:
    - Gọi các **Filter** liên quan (Authorization → Resource → Action → Result)
    - Thực thi **Action method** trong controller
    - Sau khi action hoàn tất, các Filter sau execution sẽ xử lý kết quả trước khi response gửi về client

---

## Các loại Filter chính

| Loại Filter | Mục đích chính | Interface triển khai |
|-------------|----------------|---------------------|
| **Authorization Filter** | Xác thực quyền truy cập | `IAuthorizationFilter`, `IAsyncAuthorizationFilter` |
| **Resource Filter** | Chạy trước khi model binding | `IResourceFilter`, `IAsyncResourceFilter` |
| **Action Filter** | Trước & sau khi gọi action | `IActionFilter`, `IAsyncActionFilter` |
| **Exception Filter** | Bắt & xử lý exception | `IExceptionFilter`, `IAsyncExceptionFilter` |
| **Result Filter** | Trước & sau khi trả kết quả | `IResultFilter`, `IAsyncResultFilter` |

---

## Thứ tự thực thi

Thứ tự gọi Filter khi xử lý request:

```
Authorization Filter
        ↓
Resource Filter
        ↓
Action Filter
        ↓
Controller Action
        ↓
Result Filter
        ↓
Exception Filter (khi có lỗi)
```

> **Lưu ý:** Exception Filter chỉ được kích hoạt nếu trong quá trình xử lý có exception chưa được catch.

---

## Triển khai Action Filter

Ví dụ tạo class kế thừa `IActionFilter`:

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

---

## Cách đăng ký sử dụng Filter

### 1. Dùng toàn cục (Global Filter)

Thêm filter trong `Program.cs` (hoặc `Startup.cs`):

```csharp
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new SampleFilter());
});
```

**Kết quả:** Filter này sẽ áp dụng cho **tất cả controllers và actions**.

---

### 2. Dùng cho từng Controller hoặc Action

Áp dụng bằng **Attribute**:

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

**Kết quả:** Filter chỉ áp dụng cho controller hoặc action được đánh dấu.

---

### 3. Dùng ServiceFilter hoặc TypeFilter

Nếu filter có phụ thuộc (Dependency Injection), ta dùng `ServiceFilter` hoặc `TypeFilter`.

#### a. TypeFilter

```csharp
[TypeFilter(typeof(MyCustomFilter))]
public class ProductController : Controller
{
    public IActionResult Index() => View();
}
```

#### b. ServiceFilter

```csharp
[ServiceFilter(typeof(MyCustomFilter))]
public class ProductController : Controller
{
    public IActionResult Index() => View();
}
```

**Lưu ý:** ServiceFilter yêu cầu bạn đăng ký filter trong DI container:

```csharp
builder.Services.AddScoped<MyCustomFilter>();
```

---

## Filter bất đồng bộ

Khi cần xử lý bất đồng bộ (ví dụ: ghi log vào database hoặc API ngoài), dùng `IAsyncActionFilter`:

```csharp
public class SampleAsyncFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Console.WriteLine("Before Execution");

        var resultContext = await next(); // gọi action

        Console.WriteLine("After Execution");
    }
}
```

---

## Truyền tham số vào Filter

Bạn có thể truyền giá trị từ attribute:

```csharp
public class LogActionFilter : ActionFilterAttribute
{
    private readonly string _message;

    public LogActionFilter(string message)
    {
        _message = message;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine($"Start: {_message}");
    }
}
```

**Sử dụng:**

```csharp
[LogActionFilter("Đang chạy action Index")]
public IActionResult Index() => View();
```

---

## Tình huống sử dụng thực tế

| Tình huống | Nên dùng Filter nào | Ghi chú |
|-----------|---------------------|---------|
| Ghi log thời gian chạy của Action | Action Filter | Dễ implement, can thiệp logic trước/sau Action |
| Validate dữ liệu request trước khi xử lý | Action Filter hoặc Resource Filter | Dừng pipeline nếu không hợp lệ |
| Cache kết quả của Action | Result Filter | Lưu kết quả sau khi Action hoàn thành |
| Xử lý exception chung | Exception Filter | Giảm try-catch rải rác |
| Kiểm tra quyền truy cập | Authorization Filter | Kết hợp với Authentication |

---

## So sánh tổng quan

| Thành phần | Phạm vi | Thời điểm chạy | Mục đích |
|-----------|---------|----------------|----------|
| **Middleware** | Toàn ứng dụng | Trước khi vào MVC | Xử lý logic chung như logging, auth, exception |
| **Action Filter** | Controller / Action | Trước & sau Action | Can thiệp luồng action, validate, log, cache |
| **Exception Filter** | MVC layer | Khi có lỗi | Bắt và xử lý ngoại lệ |
| **Result Filter** | Sau Action | Trước khi trả response | Sửa đổi hoặc cache kết quả |

---

## Kết luận

- **Middleware**: xử lý request ở cấp độ ứng dụng
- **Filter**: xử lý logic ở cấp độ controller/action trong MVC
- **Action Filter** giúp mã gọn hơn, tách biệt rõ logic xử lý phụ (logging, validate, timing, cache)
- Có thể kết hợp bất đồng bộ và DI để tối ưu hiệu năng và tái sử dụng

---

**Tài liệu tham khảo:**
- [Microsoft Docs - Filters in ASP.NET Core](https://docs.microsoft.com/aspnet/core/mvc/controllers/filters)
- [Action Filters Best Practices](https://docs.microsoft.com/aspnet/core/mvc/controllers/filters#action-filters)

