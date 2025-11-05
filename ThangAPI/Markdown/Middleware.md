# Middleware trong ASP.NET Core

## Mục lục
- [Giới thiệu](#giới-thiệu)
- [Chuỗi Middleware (Request Pipeline)](#chuỗi-middleware-request-pipeline)
- [app.Use vs app.Run](#appuse-vs-apprun)
- [Custom Middleware](#custom-middleware)
- [Custom Conventional Middleware](#custom-conventional-middleware)
- [Thứ tự lý tưởng của Middleware Pipeline](#thứ-tự-lý-tưởng-của-middleware-pipeline)
- [UseWhen()](#usewhen)
- [Tổng kết](#tổng-kết)

---

## Giới thiệu

Middleware trong ASP.NET Core là một chuỗi các thành phần tạo thành một pipeline mà mọi HTTP request và response đều phải đi qua. Mỗi thành phần middleware có thể:

- **Kiểm tra** incoming request
- **Chỉnh sửa** request hoặc response (nếu cần)
- **Gọi** middleware tiếp theo trong pipeline hoặc ngắt mạch và tạo response ngay lập tức

Pipeline này cho phép bạn module hóa logic ứng dụng và thêm các tính năng như authentication, logging, error handling, routing,... một cách rõ ràng và dễ bảo trì.

![Middleware Pipeline](images/middleware-pipeline.png)

---

## Chuỗi Middleware (Request Pipeline)

Hãy tưởng tượng request pipeline như một chuỗi các ống nước được kết nối. Mỗi middleware giống như một van trong pipeline này, cho phép bạn kiểm soát luồng thông tin và áp dụng các thao tác cụ thể ở các giai đoạn khác nhau. 

> ⚠️ **Lưu ý:** Thứ tự đăng ký middleware rất quan trọng vì chúng được thực thi tuần tự.

---

## app.Use vs app.Run

Hai phương thức này là nền tảng để thêm middleware vào pipeline của bạn, nhưng chúng có sự khác biệt quan trọng:

### `app.Use(async (context, next) => { ... })`

**Non-Terminal Middleware** - Middleware không kết thúc pipeline

- Thực hiện một số hành động và sau đó gọi `next` để chuyển điều khiển sang middleware tiếp theo
- Có thể chỉnh sửa Request/Response trước khi chuyển tiếp
- **Ví dụ:** Authentication, logging, custom headers, etc.

### `app.Run(async (context) => { ... })`

**Terminal Middleware** - Middleware kết thúc pipeline

- Không gọi `next`; nó kết thúc pipeline và tự tạo response
- Thường được sử dụng cho response cuối cùng
- Không thể chỉnh sửa request vì đây là điểm cuối

---

## Code Examples

### Code 1: Hậu quả của việc gọi nhiều app.Run

```csharp
app.Run(async (HttpContext context) => {
    await context.Response.WriteAsync("Hello");
});

app.Run(async (HttpContext context) => {
    await context.Response.WriteAsync("Hello again");
});

app.Run();
```

**Kết quả:** Chỉ có `app.Run` đầu tiên được thực thi. Nó kết thúc pipeline bằng cách viết "Hello" vào response, và các `app.Run` tiếp theo không bao giờ được chạy.

---

### Code 2: Chuỗi Middleware với app.Use và app.Run

```csharp
// Middleware 1
app.Use(async (context, next) => {
    await context.Response.WriteAsync("Hello ");
    await next(context);
});

// Middleware 2
app.Use(async (context, next) => {
    await context.Response.WriteAsync("Hello again ");
    await next(context);
});

// Middleware 3
app.Run(async (HttpContext context) => {
    await context.Response.WriteAsync("Hello again");
});
```

**Luồng thực thi:**
1. `app.Use` thứ nhất viết "Hello " và gọi `next`
2. `app.Use` thứ hai viết "Hello again " và gọi `next`
3. `app.Run` cuối cùng viết "Hello again" và kết thúc pipeline

**Output:** `Hello Hello again Hello again`

### Điểm cần nhớ

- ✅ Thứ tự Middleware rất quan trọng
- ✅ Sử dụng `app.Use` cho các hành động không kết thúc pipeline
- ✅ Sử dụng `app.Run` để kết thúc pipeline
- ✅ Middleware có thể short-circuit pipeline (không gọi `next`) nếu cần

---

## Custom Middleware

Mặc dù ASP.NET Core cung cấp rất nhiều middleware sẵn có, đôi khi bạn cần tạo middleware riêng để giải quyết các yêu cầu cụ thể của ứng dụng.

### Lợi ích của Custom Middleware

- **Đóng gói logic:** Gom các thao tác liên quan thành một component có thể tái sử dụng
- **Tùy chỉnh hành vi:** Điều chỉnh request/response pipeline theo đúng nhu cầu ứng dụng
- **Cải thiện tổ chức code:** Giữ code middleware sạch sẽ và dễ bảo trì

### Cấu trúc của Custom Middleware Class

#### Implement IMiddleware

Interface này yêu cầu một phương thức duy nhất: `InvokeAsync(HttpContext context, RequestDelegate next)`

```csharp
// MyCustomMiddleware.cs
namespace MiddlewareExample.CustomMiddleware
{
    public class MyCustomMiddleware : IMiddleware  
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("My Custom Middleware - Starts\n");
            await next(context);  
            await context.Response.WriteAsync("My Custom Middleware - Ends\n");
        }
    }

    // Extension method để đăng ký dễ dàng
    public static class CustomMiddlewareExtension
    {
        public static IApplicationBuilder UseMyCustomMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MyCustomMiddleware>();
        }
    }
}
```

#### Đăng ký và sử dụng

```csharp
// Program.cs (hoặc Startup.cs)
using MiddlewareExample.CustomMiddleware;

// Đăng ký service
builder.Services.AddTransient<MyCustomMiddleware>();

// Sử dụng trong pipeline
app.Use(async (HttpContext context, RequestDelegate next) => {
    await context.Response.WriteAsync("From Middleware 1\n");
    await next(context);
});

app.UseMyCustomMiddleware(); // Sử dụng extension method

app.Run(async (HttpContext context) => {
    await context.Response.WriteAsync("From Middleware 3\n");
});
```

#### Output

```
From Middleware 1
My Custom Middleware - Starts
From Middleware 3
My Custom Middleware - Ends
```

Điều này cho thấy rõ luồng thực thi qua chuỗi middleware.

---

## Custom Conventional Middleware

Conventional middleware là một cách đơn giản nhưng mạnh mẽ để đóng gói logic tùy chỉnh cho việc xử lý HTTP requests và responses.

### Đặc điểm chính

- **Class-Based:** Được implement dưới dạng class
- **Constructor Injection:** Nhận dependencies qua constructor
- **Invoke Method:** Chứa logic xử lý mỗi request
- **RequestDelegate:** Tham số đại diện cho middleware tiếp theo

### Ví dụ: HelloCustomMiddleware

```csharp
// HelloCustomMiddleware.cs
public class HelloCustomMiddleware
{
    private readonly RequestDelegate _next;

    public HelloCustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }
 
    public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext.Request.Query.ContainsKey("firstname") && 
            httpContext.Request.Query.ContainsKey("lastname"))
        {
            string fullName = httpContext.Request.Query["firstname"] + " " + 
                            httpContext.Request.Query["lastname"];
            await httpContext.Response.WriteAsync(fullName);
        }
        await _next(httpContext); 
    }
}

// Extension method để đăng ký dễ dàng
public static class HelloCustomMiddlewareExtensions
{
    public static IApplicationBuilder UseHelloCustomMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HelloCustomMiddleware>();
    }
}
```

### Sử dụng trong Program.cs

```csharp
app.UseMyCustomMiddleware();
app.UseHelloCustomMiddleware();
```

### Cách hoạt động

1. Request đến, ASP.NET Core duyệt qua middleware pipeline
2. Đến `HelloCustomMiddleware`, kiểm tra query parameters
3. Nếu có cả `firstname` và `lastname`, tạo lời chào cá nhân hóa
4. Gọi `next(context)` để chuyển request đến middleware tiếp theo

---

## Thứ tự lý tưởng của Middleware Pipeline

### 1️⃣ Exception/Error Handling
- **Mục đích:** Bắt và xử lý exceptions xảy ra ở bất kỳ đâu trong pipeline
- **Ví dụ:** `UseExceptionHandler`, `UseDeveloperExceptionPage`

### 2️⃣ HTTPS Redirection
- **Mục đích:** Chuyển hướng HTTP requests sang HTTPS để bảo mật
- **Ví dụ:** `UseHttpsRedirection`

### 3️⃣ Static Files
- **Mục đích:** Phục vụ các file tĩnh như images, CSS, JavaScript
- **Ví dụ:** `UseStaticFiles`

### 4️⃣ Routing
- **Mục đích:** Khớp incoming requests với các endpoints cụ thể
- **Ví dụ:** `UseRouting`, `UseEndpoints`

### 5️⃣ CORS (Cross-Origin Resource Sharing)
- **Mục đích:** Cho phép cross-origin requests từ các domain khác
- **Ví dụ:** `UseCors`

### 6️⃣ Authentication
- **Mục đích:** Xác minh danh tính người dùng
- **Ví dụ:** `UseAuthentication`

### 7️⃣ Authorization
- **Mục đích:** Xác định quyền truy cập tài nguyên của người dùng
- **Ví dụ:** `UseAuthorization`

### 8️⃣ Custom Middleware
- **Mục đích:** Các middleware tùy chỉnh của ứng dụng (logging, feature flags, etc.)

### Lý do đằng sau thứ tự này

- **Exception Handling sớm:** Bắt exceptions sớm để ngăn chúng lan truyền
- **Bảo mật trước:** HTTPS redirection, authentication, authorization là thiết yếu
- **Tối ưu hiệu suất:** Static files, caching được đặt sớm để tối ưu response
- **Routing là nền tảng:** Xác định cách requests được xử lý
- **CORS cho linh hoạt:** Cho phép ứng dụng được sử dụng bởi nhiều clients

### Ví dụ

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ... custom middleware của bạn ...

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
```

---

## UseWhen()

`UseWhen()` là một extension method mạnh mẽ cho phép bạn thêm middleware có điều kiện vào pipeline dựa trên một predicate.

### Cú pháp

```csharp
app.UseWhen(
    context => /* Điều kiện của bạn */,
    app => /* Cấu hình middleware cho nhánh */
);
```

### Cách hoạt động

1. **Đánh giá Predicate:** Khi request đến, `UseWhen()` đánh giá predicate
2. **Phân nhánh (nếu true):** Nếu predicate trả về `true`, nhánh middleware được thực thi
3. **Quay lại Pipeline chính:** Sau khi nhánh thực thi, request quay lại pipeline chính

### Ví dụ

```csharp
app.UseWhen(
    context => context.Request.Query.ContainsKey("username"),
    app => {
        app.Use(async (context, next) =>
        {
            await context.Response.WriteAsync("Hello from Middleware branch\n");
            await next();
        });
    });

app.Run(async context =>
{
    await context.Response.WriteAsync("Hello from middleware at main chain");
});
```

### Output

**Với query parameter `username`** (vd: `/path?username=John`):
```
Hello from Middleware branch
Hello from middleware at main chain
```

**Không có `username`** (vd: `/path`):
```
Hello from middleware at main chain
```

### Khi nào sử dụng UseWhen()?

- ✅ Bật/tắt tính năng dựa trên request
- ✅ Tạo pipeline động thích ứng với các requests khác nhau
- ✅ A/B Testing
- ✅ Áp dụng middleware chẩn đoán chỉ trong môi trường development

---

## Tổng kết

### 📌 Điểm cần nhớ

#### Hiểu biết khái niệm
- **Pipeline:** Middleware tạo thành pipeline cho HTTP requests và responses
- **Thứ tự quan trọng:** Middleware được thực thi theo thứ tự đăng ký

#### Các loại Middleware
- **Built-in:** ASP.NET Core cung cấp middleware cho authentication, routing, static files, etc.
- **Custom:** Bạn có thể tạo middleware riêng cho logic cụ thể

#### app.Use vs app.Run
| Đặc điểm | app.Use | app.Run |
|----------|---------|---------|
| Loại | Non-terminal | Terminal |
| Gọi next | ✅ Có | ❌ Không |
| Vị trí | Giữa pipeline | Cuối pipeline |
| Sử dụng | Authentication, logging | Response cuối cùng |

#### Custom Middleware - Hai cách

**1. Conventional (Class-based)**
- Sử dụng `Invoke` method
- Constructor injection

**2. Factory-Based**
- Sử dụng delegate

**Lợi ích:**
- Đóng gói logic
- Cải thiện tổ chức code
- Tùy chỉnh hành vi ứng dụng

#### Thứ tự khuyến nghị

```
1. Exception Handling
2. HTTPS Redirection
3. Static Files
4. Routing
5. CORS
6. Authentication
7. Authorization
8. Custom Middleware
9. MVC/Razor Pages/Minimal APIs
```

> ⚠️ **Lưu ý:** Đây không phải là quy tắc nghiêm ngặt, nhưng là guideline tốt!

### 🎯 Bonus Points

- **Short-Circuiting:** Middleware có thể không gọi `next` và trả về response sớm
- **UseWhen:** Thêm middleware branches có điều kiện dựa trên request criteria
- **Tính linh hoạt:** Hiểu lý do đằng sau thứ tự khuyến nghị và khi nào nên thay đổi

---

**Chúc bạn code vui vẻ! 🚀**

