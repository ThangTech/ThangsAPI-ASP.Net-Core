# Routing trong ASP.NET Core

![ASP.NET Core Routing](images/routing-diagram.png)

## Giới thiệu

Routing là cơ chế mà ASP.NET Core sử dụng để khớp các HTTP request đến với các endpoint cụ thể (ví dụ: controller actions, Razor Pages, hoặc minimal API handlers) trong ứng dụng của bạn. Điều này cho phép bạn định nghĩa các URL rõ ràng và có ý nghĩa, chỉ ra các tài nguyên hoặc hành động đang được yêu cầu.

## Cách Routing hoạt động trong ASP.NET Core

### 1. Đăng ký Endpoint

Bạn định nghĩa các endpoint (routes) trong ứng dụng, chỉ định:

- URL pattern (ví dụ: `/products`, `/api/orders/{id}`)
- HTTP method(s) mà endpoint xử lý (GET, POST, v.v.)
- Code để thực thi khi endpoint được khớp (RequestDelegate)

### 2. Request Matching (Middleware)

- Middleware `UseRouting` được thêm vào pipeline
- Khi request đến, `UseRouting` phân tích URL và HTTP method
- Nó so sánh URL với các endpoint đã đăng ký để tìm kết quả khớp tốt nhất

### 3. Endpoint Execution (Middleware)

- Middleware `UseEndpoints` được thêm vào pipeline, sau `UseRouting`
- Nếu `UseRouting` tìm thấy endpoint khớp, `UseEndpoints` thực thi code (RequestDelegate) liên kết với endpoint đó

## UseRouting vs UseEndpoints

### UseRouting

- Chịu trách nhiệm khớp route - tìm endpoint phù hợp cho request
- Thêm route data vào `HttpContext`, các middleware tiếp theo có thể sử dụng để đưa ra quyết định
- Phải đặt trước `UseEndpoints`

### UseEndpoints

- Chịu trách nhiệm thực thi endpoint - gọi code (delegate) liên kết với endpoint đã khớp
- Cho phép bạn cấu hình endpoints (ví dụ: định nghĩa policies, filters) sử dụng lambda expressions

## Map* Methods: Tạo Endpoints

ASP.NET Core cung cấp họ extension methods `Map*` trên interface `IEndpointRouteBuilder` để đơn giản hóa việc tạo endpoint:

- **MapGet**: Tạo endpoint chỉ xử lý GET requests
- **MapPost**: Tạo endpoint chỉ xử lý POST requests
- **MapPut**, **MapDelete**: Tạo endpoints cho PUT và DELETE requests
- **MapMethods**: Tạo endpoint xử lý nhiều HTTP methods
- **MapControllerRoute**, **MapAreaControllerRoute**: Dùng để cấu hình MVC/Razor Pages controllers
- **MapFallbackToFile**: Chỉ định file mặc định để serve khi không có endpoint nào khớp

### Ví dụ Code

```csharp
//enable routing
app.UseRouting();

//creating endpoints
app.UseEndpoints(endpoints =>
{
    //add your endpoints here
    endpoints.MapGet("map1", async (context) => {
        await context.Response.WriteAsync("In Map 1");
    });

    endpoints.MapPost("map2", async (context) => {
        await context.Response.WriteAsync("In Map 2");
    });
});

app.Run(async context => {
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});
```

**Giải thích:**

- `app.UseRouting();`: Kích hoạt routing middleware. Thiết lập cơ chế để phân tích requests và khớp chúng với các endpoint đã định nghĩa
- `app.UseEndpoints(endpoints => { ... });`: Lambda expression này cấu hình các endpoints:
  - `endpoints.MapGet("map1", ...);`: Đăng ký GET endpoint phản hồi path "/map1" với text "In Map 1"
  - `endpoints.MapPost("map2", ...);`: Đăng ký POST endpoint cho path "/map2", phản hồi "In Map 2"
- `app.Run(async context => { ... });`: Đây là fallback terminal middleware. Nếu không có endpoint nào khớp (ví dụ: bạn truy cập "/map3"), nó sẽ thực thi code này, ghi path được yêu cầu vào response

## GetEndpoint()

`GetEndpoint()` là một công cụ mạnh mẽ để lấy thông tin về endpoint cụ thể được chọn để xử lý HTTP request. Method này là extension method có sẵn trên object `HttpContext`.

**Mục đích**: Cho phép bạn truy cập chi tiết về endpoint đã khớp, như display name, route pattern, metadata, v.v.

**Khi nào sử dụng**: Thường sử dụng `GetEndpoint()` trong các middleware component để đưa ra quyết định dựa trên endpoint đã chọn hoặc trích xuất thông tin liên quan đến logic tùy chỉnh.

**Vị trí Middleware**: Method `GetEndpoint()` chỉ trả về object `Endpoint` hợp lệ sau khi middleware `UseRouting` đã thực thi và khớp thành công request với endpoint.

### Ví dụ Code

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Middleware 1: Before Routing
app.Use(async (context, next) =>
{
    Microsoft.AspNetCore.Http.Endpoint? endPoint = context.GetEndpoint();
    if (endPoint != null)
    {
        await context.Response.WriteAsync($"Endpoint: {endPoint.DisplayName}\n");
    }
    await next(context);
});

// Enable Routing Middleware
app.UseRouting();

// Middleware 2: After Routing
app.Use(async (context, next) =>
{
    Microsoft.AspNetCore.Http.Endpoint? endPoint = context.GetEndpoint();
    if (endPoint != null)
    {
        await context.Response.WriteAsync($"Endpoint: {endPoint.DisplayName}\n");
    }
    await next(context);
});

// Creating Endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("map1", async (context) =>
    {
        await context.Response.WriteAsync("In Map 1");
    });

    endpoints.MapPost("map2", async (context) =>
    {
        await context.Response.WriteAsync("In Map 2");
    });
});

// Fallback Middleware
app.Run(async context =>
{
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});

app.Run();
```

**Phân tích từng bước:**

1. **Middleware 1 (Before Routing)**: `GetEndpoint()` sẽ trả về `null` vì routing chưa xảy ra. Request chưa được khớp với endpoint cụ thể nào
2. **app.UseRouting();**: Kích hoạt routing middleware, chịu trách nhiệm khớp request với endpoint
3. **Middleware 2 (After Routing)**: Bây giờ, `GetEndpoint()` sẽ trả về matched endpoint object (nếu tìm thấy). Bạn có thể truy cập `DisplayName` (hoặc các thuộc tính khác) để lấy thông tin về endpoint đã chọn
   - GET request đến "/map1", display name sẽ là "map1"
   - POST request đến "/map2", display name sẽ là "map2"
   - Các path khác, display name sẽ là `null` (vì fallback middleware xử lý các trường hợp này)

## Route Parameters

Route parameters là các placeholder trong URL pattern để capture values từ incoming requests. Các giá trị này có thể được sử dụng trong endpoint handlers để tùy chỉnh response hoặc thực hiện các hành động cụ thể.

### Các loại Route Parameters

#### 1. Required Parameters (Tham số bắt buộc)

- **Cú pháp**: Đặt trong dấu ngoặc nhọn `{}`
- **Hành vi**: Phải được cung cấp trong URL để route khớp. Nếu không có, request sẽ không khớp endpoint này
- **Ví dụ**: `/products/{id}` (Tham số `id` là bắt buộc)

#### 2. Optional Parameters (Tham số tùy chọn)

- **Cú pháp**: Đặt trong dấu ngoặc nhọn `{}` và theo sau bởi dấu hỏi `?`
- **Hành vi**: Có thể bỏ qua trong URL. Nếu không có, giá trị tham số sẽ là `null`
- **Ví dụ**: `/products/details/{id?}` (Tham số `id` là tùy chọn)

#### 3. Parameters with Default Values (Tham số có giá trị mặc định)

- **Cú pháp**: Đặt trong dấu ngoặc nhọn `{}`, theo sau bởi dấu bằng `=`, và giá trị mặc định
- **Hành vi**: Nếu không cung cấp trong URL, tham số sẽ nhận giá trị mặc định đã chỉ định
- **Ví dụ**: `/employee/profile/{EmployeeName=harsha}` (Tham số `EmployeeName` mặc định là "harsha")

### Ví dụ Code

```csharp
app.UseEndpoints(endpoints =>
{
    // Required Parameters
    endpoints.Map("files/{filename}.{extension}", async context =>
    {
        string? fileName = Convert.ToString(context.Request.RouteValues["filename"]);
        string? extension = Convert.ToString(context.Request.RouteValues["extension"]);

        await context.Response.WriteAsync($"In files - {fileName} - {extension}");
    });
 
    // Default Parameter
    endpoints.Map("employee/profile/{EmployeeName=harsha}", async context =>
    {
        string? employeeName = Convert.ToString(context.Request.RouteValues["employeename"]);
        await context.Response.WriteAsync($"In Employee profile - {employeeName}");
    });
 
    // Optional Parameter
    endpoints.Map("products/details/{id?}", async context => {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Products details - {id}");
        }
        else
        {
            await context.Response.WriteAsync($"Products details - id is not supplied");
        }
    });
});
```

**Giải thích:**

- **Required Parameters**: Route `files/{filename}.{extension}` yêu cầu cả `filename` và `extension` phải có trong URL (ví dụ: `/files/sample.txt`). Endpoint handler trích xuất các giá trị này từ `context.Request.RouteValues` và sử dụng trong response
- **Default Parameter**: Route `employee/profile/{EmployeeName=harsha}` có giá trị mặc định cho `EmployeeName`
  - Truy cập `/employee/profile`, response là "In Employee profile - harsha"
  - Truy cập `/employee/profile/john`, response là "In Employee profile - john"
- **Optional Parameter**: Route `products/details/{id?}` cho phép tham số `id` được bỏ qua
  - Truy cập `/products/details/123`, hiển thị chi tiết sản phẩm ID 123
  - Truy cập `/products/details`, cho biết ID không được cung cấp

## Route Constraints

Route constraints là công cụ quan trọng trong ASP.NET Core routing cho phép bạn thêm validation bổ sung cho các route parameters. Chúng định nghĩa các quy tắc hạn chế giá trị mà parameter có thể chấp nhận, giúp lọc các request không hợp lệ trước khi chúng đến endpoint handlers.

### Tại sao sử dụng Route Constraints?

- **Tăng cường Validation**: Đảm bảo chỉ các request có giá trị parameter hợp lệ được xử lý
- **Cải thiện Bảo mật**: Ngăn chặn input độc hại bằng cách từ chối requests có giá trị có khả năng gây hại
- **Code sạch hơn**: Tránh lộn xộn endpoint handlers với logic validation
- **Routing rõ ràng**: Làm cho routes tự mô tả và dễ hiểu hơn

### Các Route Constraints phổ biến

ASP.NET Core cung cấp nhiều built-in route constraints:

- **int**: Yêu cầu giá trị parameter là integer
- **bool**: Yêu cầu giá trị parameter là boolean (true hoặc false)
- **datetime**: Yêu cầu giá trị parameter là chuỗi date time hợp lệ
- **decimal, double, float, long**: Yêu cầu giá trị parameter là kiểu số được chỉ định
- **guid**: Yêu cầu giá trị parameter là GUID hợp lệ (Globally Unique Identifier)
- **alpha**: Yêu cầu giá trị parameter chỉ bao gồm ký tự chữ cái (a-z, A-Z)
- **regex**: Yêu cầu giá trị parameter khớp với pattern regular expression
- **length**: Yêu cầu giá trị parameter có độ dài cụ thể hoặc trong khoảng được chỉ định
- **min, max, range**: Yêu cầu giá trị parameter lớn hơn hoặc bằng minimum (min), nhỏ hơn hoặc bằng maximum (max), hoặc trong khoảng cụ thể (range)

### Ví dụ Code

```csharp
app.UseEndpoints(endpoints =>
{
    // Alphabetic and Length Constraint
    endpoints.Map("employee/profile/{EmployeeName:length(4,7):alpha=harsha}", async context =>
    {
        string? employeeName = Convert.ToString(context.Request.RouteValues["employeename"]);
        await context.Response.WriteAsync($"In Employee profile - {employeeName}");
    });
 
    // Integer, Range, and Optional Constraint
    endpoints.Map("products/details/{id:int:range(1,1000)?}", async context => {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Products details - {id}");
        }
        else
        {
            await context.Response.WriteAsync($"Products details - id is not supplied");
        }
    });
 
    // DateTime Constraint
    endpoints.Map("daily-digest-report/{reportdate:datetime}", async context =>
    {
        DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
        await context.Response.WriteAsync($"In daily-digest-report - {reportDate}");
    });
 
    // GUID Constraint
    endpoints.Map("cities/{cityid:guid}", async context =>
    {
        Guid cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"]));
        await context.Response.WriteAsync($"City information - {cityId}");
    });
 
    // Int, Min, Regex Constraint
    endpoints.Map("sales-report/{year:int:min(1900)}/{month:regex(^(apr|jul|oct|jan)$)}", async context =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);
        await context.Response.WriteAsync($"Sales report - {year} - {month}");
    });
});
```

**Giải thích:**

- **Alphabetic và Length Constraint**: `/employee/profile/{EmployeeName:length(4,7):alpha=harsha}` - Đảm bảo `EmployeeName` dài 4-7 ký tự và chỉ bao gồm ký tự chữ cái. Nếu không cung cấp, mặc định là "harsha"
- **Integer, Range và Optional Constraint**: `/products/details/{id:int:range(1,1000)?}` - Yêu cầu `id` là integer từ 1 đến 1000. Dấu hỏi làm cho nó tùy chọn
- **DateTime Constraint**: `/daily-digest-report/{reportdate:datetime}` - Yêu cầu `reportdate` là chuỗi date-time hợp lệ
- **GUID Constraint**: `/cities/{cityid:guid}` - Yêu cầu `cityid` là GUID hợp lệ
- **Integer, Min và Regex Constraint**: `/sales-report/{year:int:min(1900)}/{month:regex(^(apr|jul|oct|jan)$)}` - Yêu cầu `year` là integer >= 1900, và `month` là một trong các giá trị được chỉ định (apr, jul, oct, jan)

## Custom Route Constraint Classes

Trong khi ASP.NET Core cung cấp nhiều built-in route constraints, đôi khi ứng dụng của bạn yêu cầu các quy tắc validation chuyên biệt hơn. Custom route constraint classes cho phép bạn định nghĩa tiêu chí riêng để xác định xem giá trị parameter có hợp lệ hay không.

### Yêu cầu chính

1. **Implement IRouteConstraint**: Tạo class implement interface `IRouteConstraint`
2. **Match Method**: Implement method `Match`, chứa logic validation tùy chỉnh. Method này nhận các tham số:
   - `httpContext`: `HttpContext` hiện tại
   - `route`: Object `IRouter` liên kết với route
   - `routeKey`: Tên của route parameter đang được validate
   - `values`: Dictionary chứa các route values
   - `routeDirection`: Chỉ ra route đang được khớp cho incoming request hay để tạo URL
3. **Return true hoặc false**: Method `Match` phải trả về `true` nếu giá trị parameter hợp lệ theo constraint của bạn, và `false` nếu ngược lại

### Ví dụ Code

```csharp
// MonthsCustomConstraint.cs
public class MonthsCustomConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, 
                      RouteValueDictionary values, RouteDirection routeDirection)
    {
        // Check if the parameter value exists
        if (!values.ContainsKey(routeKey))
        {
            return false; // Not a match
        }

        Regex regex = new Regex("^(apr|jul|oct|jan)$");
        string? monthValue = Convert.ToString(values[routeKey]);
 
        if (regex.IsMatch(monthValue))
        {
            return true; // It's a match
        }
        return false; // Not a match
    }
}
```

**Phân tích:**

- **Implementation của IRouteConstraint**: Class `MonthsCustomConstraint` rõ ràng implements interface này, báo hiệu rằng đây là custom route constraint
- **Match Method**:
  - Đầu tiên kiểm tra xem dictionary `values` có chứa route parameter đang được validate (`routeKey`) không. Nếu không, trả về `false` ngay lập tức
  - Một regular expression `^(apr|jul|oct|jan)$` được sử dụng để định nghĩa các giá trị month hợp lệ
  - Giá trị liên kết với `routeKey` được lấy từ dictionary `values` và chuyển thành string
  - Method `Regex.IsMatch` kiểm tra xem giá trị có khớp với pattern month được phép không
  - Trả về `true` nếu giá trị khớp, và `false` nếu ngược lại

### Sử dụng Custom Constraint

```csharp
// Đăng ký custom constraint
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("months", typeof(MonthsCustomConstraint));
});

// Sử dụng trong endpoint configuration
app.UseEndpoints(endpoints =>
{
    endpoints.Map("sales-report/{year:int:min(1900)}/{month:months}", async context =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);
        await context.Response.WriteAsync($"Sales report - {year} - {month}");
    });
});
```

Chú ý constraint `:months` sau parameter `month`. Điều này chỉ ra rằng giá trị cho `month` nên được validate theo class `MonthsCustomConstraint`.

## Endpoint Selection

Khi request đến ứng dụng ASP.NET Core, routing middleware (`UseRouting`) phân tích URL và HTTP method. Sau đó nó so sánh thông tin này với collection các endpoints bạn đã định nghĩa sử dụng các methods như `MapGet`, `MapPost`, v.v. Mục tiêu là tìm endpoint phù hợp nhất để xử lý request.

Tuy nhiên, điều gì xảy ra khi nhiều endpoints có vẻ như là potential matches? ASP.NET Core sử dụng một thuật toán được định nghĩa rõ ràng để xác định winning endpoint.

### Thuật toán Endpoint Selection

#### 1. Precedence (Độ ưu tiên)

- **Explicit Matches**: Endpoints được định nghĩa với pattern cụ thể hơn (ví dụ: `/products/{id}`) được ưu tiên hơn những cái có pattern rộng hơn (ví dụ: `/products`)
- **Order of Registration**: Nếu nhiều endpoints có pattern cụ thể ngang nhau có thể khớp, endpoint được đăng ký đầu tiên sẽ thắng

#### 2. HTTP Method

- **Exact Match**: Nếu request method (GET, POST, v.v.) khớp chính xác với method được chỉ định cho endpoint, endpoint đó được ưu tiên

#### 3. Route Constraints

- **More Specific Constraints**: Endpoints với route constraints hạn chế hơn (ví dụ: `id:int:range(1,100)` vs `id:int`) được ưu tiên

#### 4. Catch-All (Fallback)

- Nếu không có endpoint nào khớp, và bạn có catch-all endpoint (được định nghĩa sử dụng `MapFallback`), nó sẽ được chọn

### Thứ tự Ưu tiên

1. Explicit Match với Exact HTTP Method và More Specific Route Constraints
2. Explicit Match với Exact HTTP Method và Less Specific Route Constraints
3. Explicit Match với Any HTTP Method và More Specific Route Constraints
4. Explicit Match với Any HTTP Method và Less Specific Route Constraints
5. Order of Registration (nếu độ cụ thể bằng nhau)
6. Catch-All Endpoint (nếu không tìm thấy match nào khác)

### Ví dụ thực tế

```csharp
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/products/{id:int}", GetProductById);      // Most specific
    endpoints.MapGet("/products", GetAllProducts);               // Less specific
    endpoints.MapGet("/{path?}", CatchAllHandler);               // Catch-all
});
```

Trong ví dụ này:

- `/products/123` sẽ khớp endpoint đầu tiên (`GetProductById`)
- `/products` sẽ khớp endpoint thứ hai (`GetAllProducts`)
- `/anything-else` sẽ khớp catch-all endpoint (`CatchAllHandler`)

### Lưu ý và Best Practices

- **Mind Your Order**: Chú ý thứ tự bạn đăng ký endpoints, đặc biệt nếu chúng có pattern tương tự
- **Specificity Wins**: Định nghĩa routes cụ thể nhất có thể để tránh mơ hồ
- **Route Constraints**: Sử dụng route constraints để thu hẹp giá trị hợp lệ cho parameters
- **Catch-All with Caution**: Catch-all endpoints có thể hữu ích, nhưng sử dụng tiết kiệm để tránh khớp không mong muốn
- **Endpoint Metadata**: Khám phá metadata của object `Endpoint` để hiểu rõ tại sao endpoint cụ thể được chọn

### Giải quyết Ambiguity

Nếu routing system không thể xác định được best match một cách dứt khoát, bạn sẽ gặp `AmbiguousMatchException`. Exception này báo hiệu rằng bạn cần tinh chỉnh route definitions hoặc registration order để loại bỏ conflict.

## Static Files trong ASP.NET Core

Static files là các assets tạo nên phần trình bày trực quan và chức năng của ứng dụng web:

- **HTML Files**: Cấu trúc của các web pages
- **CSS Stylesheets**: Styling và appearance của nội dung
- **JavaScript Files**: Các phần tử tương tác và logic của ứng dụng
- **Images**: Các phần tử trực quan nâng cao trải nghiệm người dùng

ASP.NET Core cung cấp middleware component `UseStaticFiles()` để phục vụ hiệu quả các static files này trực tiếp đến trình duyệt mà không cần bất kỳ server-side processing nào.

### WebRoot: Vị trí mặc định

Thuộc tính `WebRoot` trong ASP.NET Core chỉ định thư mục mặc định từ đó static files được phục vụ. Mặc định, thư mục này có tên "wwwroot" và nằm ở root của project. Tuy nhiên, bạn có thể tùy chỉnh vị trí này nếu cần.

### UseStaticFiles() Middleware

#### Sử dụng cơ bản

Gọi `app.UseStaticFiles();` không có tham số sẽ phục vụ static files từ thư mục `WebRoot` mặc định.

#### Tùy chỉnh

Bạn có thể tùy chỉnh hành vi của `UseStaticFiles()` bằng cách truyền object `StaticFileOptions`:

- **FileProvider**: Chỉ định file provider khác (ví dụ: `PhysicalFileProvider`) để phục vụ files từ vị trí tùy chỉnh
- **RequestPath**: Cấu hình base URL path cho static files (ví dụ: `/static`)
- **ContentTypeProvider**: Tùy chỉnh cách xác định content types cho các file extensions khác nhau
- **OnPrepareResponse**: Thực hiện các hành động bổ sung trên response trước khi gửi đến client

### Ví dụ Code

```csharp
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    WebRootPath = "myroot"
});
var app = builder.Build();

// Serve from the specified WebRoot ("myroot" in this case)
app.UseStaticFiles();

// Serve from a custom directory ("mywebroot") located within the project's ContentRootPath
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "mywebroot")
    )
});

// ... (rest of your middleware and endpoints) ...
app.Run();
```

**Giải thích:**

- **Custom WebRoot**: Thuộc tính `WebRootPath` trong `WebApplicationOptions` được set thành "myroot", làm "myroot" trở thành vị trí mặc định cho static files được phục vụ bởi `app.UseStaticFiles()` đầu tiên
- **Default Static Files**: Lệnh gọi `app.UseStaticFiles();` đầu tiên phục vụ files trực tiếp từ thư mục "myroot". Ví dụ, request đến `/styles.css` sẽ tìm file tên `styles.css` trong "myroot"
- **Custom Static Files Location**: Lệnh gọi `app.UseStaticFiles` thứ hai cấu hình `PhysicalFileProvider` để phục vụ files từ vị trí tùy chỉnh: "mywebroot". Thư mục này nằm trong `ContentRootPath` của ứng dụng (thư mục root của project)

### Lưu ý quan trọng

- **Bảo mật**: Luôn cẩn thận về các files bạn expose như static content. Tránh đặt thông tin nhạy cảm trong `WebRoot` hoặc thư mục tùy chỉnh
- **Hiệu năng**: Cân nhắc sử dụng các kỹ thuật caching và compression để tối ưu hóa việc phân phối static files
- **Content Security Policy (CSP)**: Implement CSP để giảm thiểu các tấn công cross-site scripting (XSS) có thể khai thác static files của bạn

## Điểm chính cần nhớ

### Routing

- **Mục đích**: Khớp incoming HTTP requests với các endpoint cụ thể (controllers, Razor Pages, minimal APIs) trong ứng dụng
- **Middleware**: `UseRouting` và `UseEndpoints` là các middleware component thiết yếu cho routing
  - `UseRouting`: Phân tích request URL và khớp với endpoint
  - `UseEndpoints`: Thực thi code của matched endpoint
- **Map* Methods**: Dùng để định nghĩa endpoints cho các HTTP methods khác nhau (ví dụ: `MapGet`, `MapPost`, `MapControllerRoute`)

### Thứ tự Endpoint Selection

1. **Specificity**: Routes cụ thể hơn (với nhiều parameters hoặc constraints hơn) được ưu tiên hơn các routes ít cụ thể hơn
2. **Registration Order**: Nếu nhiều routes cụ thể ngang nhau, route được đăng ký đầu tiên thắng
3. **HTTP Method**: Routes với method match chính xác được ưu tiên
4. **Route Constraints**: Routes với constraints hạn chế hơn được ưu tiên
5. **Catch-All**: Fallback endpoint xử lý các requests không khớp

### Route Parameters

- **Types**: Required (`{id}`), optional (`{id?}`), default value (`{id=123}`)
- **Access**: Giá trị parameters được truy cập qua `context.Request.RouteValues`

### Route Constraints

- **Mục đích**: Hạn chế các giá trị được phép cho route parameters
- **Built-in**: int, bool, datetime, guid, regex, length, min, max, range, v.v.
- **Custom**: Tạo classes implement `IRouteConstraint` để định nghĩa logic validation riêng

### GetEndpoint()

- **Mục đích**: Lấy thông tin về matched endpoint
- **Sử dụng**: Gọi `context.GetEndpoint()` trong middleware sau `UseRouting`
- **Thông tin**: Truy cập các thuộc tính endpoint như `DisplayName`, route pattern, và metadata

### Static Files

- **WebRoot**: Thư mục mặc định từ đó static files được phục vụ (thường là "wwwroot")
- **UseStaticFiles()**: Middleware để phục vụ static files (HTML, CSS, JavaScript, images)
- **Tùy chỉnh**: Sử dụng `StaticFileOptions` để thay đổi file provider, request path, hoặc các settings khác

## Mẹo phỏng vấn chính

- **Giải thích Flow**: Diễn đạt rõ ràng cách request flow qua routing middleware và cách endpoints được chọn
- **Code Examples**: Chuẩn bị viết code snippets minh họa endpoint registration, parameter usage, và constraint application
- **Troubleshooting**: Giải thích cách bạn chẩn đoán và sửa các vấn đề routing phổ biến (ví dụ: lỗi 404, ambiguous matches)
- **Best Practices**: Thảo luận cách thiết kế routes clean, maintainable và secure

