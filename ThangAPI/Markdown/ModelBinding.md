# Model Binding

Model binding là một tính năng mạnh mẽ trong ASP.NET Core MVC giúp tự động trích xuất dữ liệu từ các phần khác nhau của HTTP request (form data, route values, query strings) và chuyển đổi chúng thành các đối tượng C# có kiểu mạnh mà bạn có thể sử dụng trực tiếp trong action methods.

## Khi Nào Model Binding Được Thực Thi

Model binding diễn ra sau khi routing đã xác định action method nào sẽ được gọi. Hệ thống model binding kiểm tra các tham số của action method và cố gắng gán giá trị từ request đến.



## Thứ Tự Model Binding

ASP.NET Core model binding tuân theo thứ tự cụ thể khi tìm kiếm nguồn dữ liệu:

1. **Form Data** (POST requests): Giá trị gửi qua HTML forms
2. **Route Data**: Giá trị trích xuất từ URL route template (ví dụ: `/products/{id}`)
3. **Query String**: Giá trị thêm vào URL sau dấu `?`



## Các Phần Của Model Binding

- **Form Data**: Thường dùng để gửi dữ liệu từ HTML forms sử dụng POST requests
- **Route Data**: Giá trị lấy từ các segment URL được định nghĩa trong route templates
- **Query String**: Tham số truyền trong URL sau dấu `?`



## Query Strings Chi Tiết

**Mục đích**: Truyền tham số đến ứng dụng thông qua URL

**Cú pháp**: `?key1=value1&key2=value2` (nhiều cặp key-value phân tách bởi dấu &)

**Sử dụng**: Hữu ích cho filtering, sorting và pagination

**Ví dụ**: `/products?category=electronics&sort=price_desc`

### Best Practices (Query Strings)

- **Hạn chế dữ liệu nhạy cảm**: Tránh truyền thông tin nhạy cảm như mật khẩu qua query strings
- **Sanitize Input**: Luôn kiểm tra và xác thực giá trị query string để tránh lỗ hổng bảo mật
- **Giữ đơn giản**: Sử dụng tên tham số rõ ràng và có ý nghĩa. Tránh query strings quá dài
- **Encoding**: Mã hóa đúng cách các ký tự đặc biệt trong giá trị query string

### Những Điều Cần Tránh

- **Dữ liệu nhạy cảm**: Không bao giờ truyền mật khẩu hoặc authentication tokens trong query strings
- **Đối tượng phức tạp**: Tránh truyền đối tượng phức tạp do giới hạn độ dài URL
- **Lạm dụng**: Đừng làm quá tải URL với quá nhiều tham số query



## Route Data Chi Tiết

**Mục đích**: Lấy các giá trị động từ URL dựa trên route template

**Cú pháp**: `/products/{id}`, trong đó `id` là route parameter

**Sử dụng**: Thiết yếu cho RESTful APIs và tạo URL sạch, dễ đọc

**Ví dụ**: `/products/12345` (12345 sẽ là giá trị của tham số `id`)

### Best Practices (Route Data)

- **Chọn tên rõ ràng**: Sử dụng tên mô tả cho route parameters
- **Constraints**: Áp dụng route constraints (ví dụ: `int`, `guid`) để đảm bảo kiểu dữ liệu hợp lệ
- **Custom Constraints**: Tạo custom constraints cho validation phức tạp hơn

### Ví Dụ Code

```csharp
// HomeController.cs
[Route("bookstore/{bookid?}/{isloggedin?}")] // Route với tham số tùy chọn
public IActionResult Index(int? bookid, bool? isloggedin)
{
    // ... validation logic ...
    return Content($"Book id: {bookid}, isloggedin: {isloggedin}", "text/plain");
}
```

Trong action method này:
- `bookid` và `isloggedin` được tự động bind từ query string và route data

```csharp
// StoreController.cs
[Route("store/books/{id}")] // Route với tham số bắt buộc
public IActionResult Books()
{
    int id = Convert.ToInt32(Request.RouteValues["id"]);
    return Content($"<h1>Book Store {id}</h1>", "text/html");
}
```

Trong action method này:
- `id` là tham số bắt buộc
- `id` được bind từ route data (`Request.RouteValues`)







## [FromQuery] và [FromRoute]

Mặc dù ASP.NET Core tự động cố gắng khớp tham số action method với các phần khác nhau của request (form data, route values, query strings), bạn có thể sử dụng attributes `[FromQuery]` và `[FromRoute]` để chỉ định rõ ràng nơi model binder tìm kiếm giá trị.

### [FromQuery]

**Mục đích**: Chỉ định model binder lấy giá trị từ query string

**Sử dụng**: Áp dụng cho tham số action method mà bạn muốn nhận giá trị từ query string (phần sau dấu "?" trong URL)

**Ví dụ**:
```csharp
public IActionResult Index([FromQuery] int page) { ... }
```

Trong ví dụ này, tham số `page` sẽ được bind với giá trị của query parameter `page` trong URL (ví dụ: `/products?page=3`)

### [FromRoute]

**Mục đích**: Chỉ định model binder lấy giá trị từ route data

**Sử dụng**: Áp dụng cho tham số action method mà bạn muốn nhận giá trị từ route template của URL

**Ví dụ**:
```csharp
[Route("products/{id}")]
public IActionResult Details([FromRoute] int id) { ... }
```

Trong ví dụ này, tham số `id` sẽ được bind với giá trị của segment `id` trong URL (ví dụ: `/products/123`)

### Ví Dụ Kết Hợp

```csharp
// HomeController.cs
[Route("bookstore/{bookid?}/{isloggedin?}")]
public IActionResult Index([FromQuery] int? bookid, [FromRoute] bool? isloggedin)
{
    // ... validation và response logic ...
}
```

Trong code này:
- `bookid` (FromQuery): Model binder sẽ lấy giá trị `bookid` **chỉ từ query string**
- `isloggedin` (FromRoute): Model binder sẽ tìm giá trị `isloggedin` **trong route data**

### Lưu Ý

- **Explicit Binding**: Sử dụng `[FromQuery]` và `[FromRoute]` để kiểm soát rõ ràng nguồn dữ liệu
- **Flexibility**: Có thể kết hợp cả hai attributes trong cùng một action
- **Default Behavior**: Ngay cả khi không có attributes này, ASP.NET Core vẫn tự động xác định nguồn binding
- **Type Conversion**: Model binder tự động chuyển đổi giá trị sang kiểu dữ liệu phù hợp









## Model Classes

Trong ASP.NET Core MVC, model classes là nền tảng để đại diện cho dữ liệu mà ứng dụng của bạn làm việc. Chúng thường phản ánh cấu trúc dữ liệu từ database, API hoặc các nguồn khác.

### Mục Đích

- **Structure**: Cung cấp cấu trúc rõ ràng cho dữ liệu của bạn
- **Validation**: Áp dụng các quy tắc validation bằng attributes như `[Required]`, `[StringLength]`, `[Range]`
- **Organization**: Giữ cho logic dữ liệu của ứng dụng được tổ chức và dễ bảo trì

### Ví Dụ Model Class

```csharp
// Book.cs (Model)
namespace IActionResultExample.Models
{
    public class Book
    {
        public int? BookId { get; set; }
        public string? Author { get; set; }

        public override string ToString()
        {
            return $"Book object - Book id: {BookId}, Author: {Author}";
        }
    }
}
```

## Model Binding Với Model Classes

Model binding với model classes đơn giản hóa việc điền dữ liệu vào model objects từ HTTP requests.

### Cách Hoạt Động

1. **Action Parameter**: Khai báo tham số action method với kiểu model class
2. **Model Binding**: Model binder tự động map dữ liệu request vào properties của model class dựa trên tên
3. **Attribute Usage**: Có thể dùng `[FromQuery]`, `[FromRoute]`, `[FromBody]` để chỉ định nguồn dữ liệu

### Ví Dụ

```csharp
// HomeController.cs
[Route("bookstore/{bookid?}/{isloggedin?}")]
// Url: /bookstore/1/false?bookid=20&isloggedin=true&author=harsha
public IActionResult Index([FromQuery] int? bookid, [FromRoute] bool? isloggedin, Book book)
{
    // ... validation logic ...
    return Content($"Book id: {bookid}, Book: {book}", "text/plain");
}
```

Trong code này:
- `book.Author` sẽ được lấy từ query string (vì không có attribute nào được chỉ định)
- Model binder tự động điền dữ liệu vào object `book`

### Lợi Ích

- **Simplified Code**: Giảm code boilerplate
- **Strong Typing**: Làm việc với strongly typed model objects
- **Clear Intent**: Attributes làm code rõ ràng hơn
- **Automatic Conversion**: Model binder tự động chuyển đổi kiểu dữ liệu







## URL Encoding

URL encoding (percent-encoding) là cơ chế mã hóa các ký tự đặc biệt trong URL.

**Đặc điểm**:
- Ký tự đặc biệt: Khoảng trắng, `&`, `?` và các ký tự non-ASCII cần được mã hóa
- Format: Ký tự đặc biệt được thay bằng `%` + 2 chữ số hexadecimal
- Ví dụ: Khoảng trắng = `%20`, dấu `&` = `%26`

## Content Types Cho Form Submission

### application/x-www-form-urlencoded

- **Mục đích**: Encoding mặc định cho HTML forms
- **Cách hoạt động**: Encode form data thành cặp key-value phân tách bởi `&`
- **Sử dụng**: Phù hợp cho forms đơn giản với text data
- **Hạn chế**: Không hiệu quả với file uploads

### multipart/form-data

- **Mục đích**: Dùng cho forms với files hoặc dữ liệu lớn
- **Đặc điểm**: Mỗi field được gửi riêng biệt với content type riêng
- **Sử dụng**: Bắt buộc cho file uploads
- **Lợi ích**: Xử lý binary data hiệu quả

### form-data

- **Mục đích**: Format mới và linh hoạt hơn cho form submissions
- **Đặc điểm**: Xử lý cả simple và complex data, bao gồm files
- **Lợi ích**: Cấu trúc streamlined hơn `multipart/form-data`







## Model Validation

Model validation là quá trình xác minh dữ liệu được gửi đến ứng dụng ASP.NET Core MVC của bạn có đáp ứng các tiêu chí đã định nghĩa hay không.

### Tại Sao Model Validation Quan Trọng

- **Bảo mật**: Bảo vệ chống lại các cuộc tấn công như SQL injection, XSS và overposting
- **Tính toàn vẹn dữ liệu**: Đảm bảo dữ liệu được lưu trữ là hợp lệ
- **Trải nghiệm người dùng**: Cung cấp phản hồi ngay lập tức để người dùng sửa lỗi

### Best Practices

- **Validate cả hai phía**: Validate ở client-side (JavaScript) và server-side
- **Sử dụng Data Annotations**: Tận dụng attributes từ `System.ComponentModel.DataAnnotations`
- **Custom Validation**: Tạo custom validation attributes cho các quy tắc phức tạp
- **Model State**: Luôn kiểm tra `ModelState.IsValid` trước khi xử lý dữ liệu
- **Hiển thị lỗi**: Hiển thị rõ ràng thông báo lỗi cho người dùng

### Data Annotations Thông Dụng

- `[Required]`: Trường không được null hoặc rỗng
- `[StringLength]`: Giới hạn độ dài string
- `[Range]`: Chỉ định khoảng giá trị số
- `[RegularExpression]`: Validate theo regex pattern
- `[EmailAddress]`: Xác minh định dạng email
- `[Compare]`: So sánh giá trị của hai property
- `[Phone]`: Validate định dạng số điện thoại
- `[Url]`: Validate định dạng URL









## Model State trong Controllers

Object `ModelState` trong controllers theo dõi trạng thái validation của model sau khi model binder điền dữ liệu từ request.

**Các thuộc tính chính**:
- `ModelState.IsValid`: Boolean cho biết validation có pass hay không
- `ModelState.AddModelError`: Thêm lỗi thủ công cho property cụ thể

### Ví Dụ Code

```csharp
// Person.cs (Model)
public class Person
{
    // ... other properties

    [Required(ErrorMessage = "{0} can't be blank")]
    [Compare("Password", ErrorMessage = "{0} and {1} do not match")]
    [Display(Name = "Re-enter Password")]
    public string? ConfirmPassword { get; set; }
 
    // ... other properties
}

// Controller action
public IActionResult Create(Person person)
{
    if (!ModelState.IsValid)
    {
        return View(person); // Trả về view với validation errors
    }

    // Model hợp lệ, tiếp tục xử lý
}
```

Trong code này:
- **Data Annotations**: Model `Person` sử dụng data annotations để áp dụng validation rules
- **Model State Check**: Controller kiểm tra `ModelState.IsValid`. Nếu false, view được render lại với errors
- **Error Display**: View sử dụng `@Html.ValidationSummary()` và `@Html.ValidationMessageFor()` để hiển thị errors







## Custom Validation với ValidationAttribute

ASP.NET Core cho phép tạo custom validation attributes khi các validation có sẵn không đáp ứng được yêu cầu nghiệp vụ.

### Các Bước Chính

1. **Kế thừa ValidationAttribute**: Tạo class kế thừa từ `ValidationAttribute`
2. **Override IsValid**: Viết logic validation trong method `IsValid`
3. **Trả về ValidationResult**: 
   - `ValidationResult.Success` nếu hợp lệ
   - `new ValidationResult(message)` nếu không hợp lệ

### Ví Dụ Đơn Giản

```csharp
public class MinimumYearValidatorAttribute : ValidationAttribute
{
    public int MinimumYear { get; set; } = 2000;
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null)
        {
            DateTime date = (DateTime)value;
            if (date.Year >= MinimumYear)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"Year should not be less than {MinimumYear}");
            }
        }
        return null;
    }
}
```

**Sử dụng**: Đảm bảo một ngày không nhỏ hơn năm được chỉ định









## IValidatableObject

Interface `IValidatableObject` cho phép thực hiện validation phức tạp trên toàn bộ model, không chỉ từng property riêng lẻ.

### Khi Nào Sử Dụng

- **Cross-Property Validation**: Khi validation phụ thuộc vào nhiều properties (ví dụ: "Start Date" phải trước "End Date")
- **Business Rules phức tạp**: Khi có logic validation phức tạp hoặc cần truy vấn database
- **Custom Error Messages**: Khi cần kiểm soát chi tiết thông báo lỗi

### Ví Dụ

```csharp
public class Person : IValidatableObject
{
    public DateTime? DateOfBirth { get; set; }
    public int? Age { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DateOfBirth.HasValue == false && Age.HasValue == false)
        {
            yield return new ValidationResult(
                "Either of Date of Birth or Age must be supplied", 
                new[] { nameof(Age) }
            );
        }
    }
}
```

**Lưu ý**: Method `Validate` được gọi sau khi tất cả validation attributes đã được kiểm tra









## [Bind] và [BindNever] Attributes

Sử dụng để kiểm soát chính xác properties nào được bind từ request data.

### [Bind] Attribute

**Mục đích**: Chỉ định rõ ràng các properties được phép bind

**Ví dụ**:
```csharp
[HttpPost]
public IActionResult Create([Bind("Title", "Description")] Product product)
{
    // Chỉ Title và Description được bind từ request
}
```

### [BindNever] Attribute

**Mục đích**: Loại trừ properties khỏi model binding

**Ví dụ**:
```csharp
public class Product
{
    [BindNever]
    public DateTime CreatedAt { get; set; }  // Không bao giờ bind từ request
}
```

**Lợi ích bảo mật**: `[BindNever]` giúp ngăn chặn overposting attacks







## [FromBody] Attribute

Attribute `[FromBody]` được dùng để bind dữ liệu từ request body (thường là JSON hoặc XML).

### Cách Hoạt Động

1. **Request Body Identification**: Middleware kiểm tra `Content-Type` header
2. **Input Formatter Selection**: Chọn formatter phù hợp (JSON hoặc XML)
3. **Model Binding**: Deserialize dữ liệu và map vào model class
4. **Validation**: Validate model sau khi bind

### Ví Dụ

```csharp
// HomeController.cs
[Route("register")]
public IActionResult Index([FromBody] Person person)
{
    if (!ModelState.IsValid)
    {
        // Xử lý validation errors
    }
    return Content($"{person}"); 
}
```

**Sample JSON Request**:
```json
{
    "PersonName": "William",
    "Email": "william@example.com",
    "Phone": "123456",
    "Password": "william123",
    "ConfirmPassword": "william123"
}
```

### Lưu Ý Quan Trọng

- Chỉ một parameter per action có thể dùng `[FromBody]`
- `Content-Type` header phải khớp với format mong đợi
- Luôn validate và sanitize dữ liệu từ request body







## Input Formatters

Input formatters chịu trách nhiệm deserialize dữ liệu từ request body.

### Các Input Formatters Phổ Biến

- **NewtonsoftJsonInputFormatter**: Xử lý JSON với thư viện Newtonsoft.Json
- **SystemTextJsonInputFormatter**: Xử lý JSON với System.Text.Json (built-in)
- **XmlSerializerInputFormatter**: Xử lý XML data

### Cấu Hình

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddXmlSerializerFormatters();
```

Thêm `.AddXmlSerializerFormatters()` để hỗ trợ XML requests.







## Custom Model Binders (Nâng Cao)

Bạn có thể tạo custom model binders khi cần xử lý dữ liệu phức tạp mà default model binder không hỗ trợ.

### Khi Nào Cần

- Xử lý format dữ liệu đặc biệt
- Áp dụng business rules phức tạp trong quá trình binding
- Kiểm soát hoàn toàn quá trình binding

**Lưu ý**: Đây là tính năng nâng cao, chỉ sử dụng khi thực sự cần thiết.









## Collection Binding

ASP.NET Core model binding có thể xử lý collections như lists và arrays.

### Cách Hoạt Động

1. **Collection Property**: Model class có property kiểu collection (ví dụ: `List<T>`, `T[]`)
2. **Naming Convention**: Request parameters theo naming convention cụ thể
3. **Automatic Binding**: Model binder tự động nhận diện và điền dữ liệu

### Naming Conventions

- **Indexed**: `items[0]`, `items[1]`, `items[2]`, ... (cho lists và arrays)
- **Same Name**: `items`, `items`, `items`, ... (cho ICollection<T>)

### Ví Dụ

```csharp
// Person.cs (Model)
public class Person
{
    // ... other properties
    public List<string?> Tags { get; set; } = new List<string?>();
}

// HomeController.cs
public IActionResult Index(Person person)
{
    return Content($"Person: {person}, Tags: {string.Join(",", person.Tags)}");
}
```

**Sample JSON Request**:
```json
{
    "PersonName": "Alice",
    "Email": "alice@example.com",
    "Tags": ["music", "reading", "coding"]
}
```

**Response**:
```
Person: Alice, Tags: music,reading,coding
```









## [FromHeader] Attribute

Attribute `[FromHeader]` dùng để lấy giá trị từ HTTP request headers.

### Tại Sao Sử Dụng?

- **Access to Metadata**: Headers chứa thông tin về client, request và data
- **Custom Parameters**: Định nghĩa custom headers để truyền thêm dữ liệu
- **Security**: Headers thường dùng cho authentication tokens
- **Content Negotiation**: Headers như `Accept` xác định format response

### Ví Dụ

```csharp
// HomeController.cs
[Route("register")]
public IActionResult Index(Person person, [FromHeader(Name = "User-Agent")] string UserAgent)
{
    if (!ModelState.IsValid)
    {
        // Xử lý validation errors
    }
    return Content($"{person}, {UserAgent}");
}
```

**Sample Header trong Postman**:
```
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36
```

### Lưu Ý

- Header names không phân biệt chữ hoa/thường
- Có thể dùng `[FromHeader]` cho nhiều parameters
- Sử dụng `?` cho default values (ví dụ: `string? UserAgent`)







## Tóm Tắt Các Điểm Chính

### 1. Model Binding: Kết Nối HTTP và C#

**Mục đích**: Tự động map dữ liệu từ HTTP requests (form data, route values, query strings, headers, body) vào action method parameters hoặc model properties.

**Lợi ích**: 
- Giảm code boilerplate
- Cung cấp strong typing
- Đơn giản hóa xử lý dữ liệu

**Quy trình**:
1. Phân tích request
2. Tạo value provider để truy cập dữ liệu từ các nguồn khác nhau
3. Chọn model binder phù hợp
4. Map giá trị vào model properties dựa trên tên và attributes

### 2. Model Validation: Đảm Bảo Tính Toàn Vẹn Dữ Liệu

**Mục đích**: Đảm bảo dữ liệu gửi đến ứng dụng đáp ứng các tiêu chí đã định trước khi xử lý.

**Tại sao quan trọng**: Tăng cường bảo mật, duy trì tính toàn vẹn dữ liệu, cải thiện trải nghiệm người dùng.

**Các cách tiếp cận**:
- **Data Annotations**: Sử dụng attributes như `[Required]`, `[StringLength]`, `[Range]`
- **IValidatableObject**: Implement interface này để thực hiện custom model-level validation
- **Custom Validation Attributes**: Tạo attributes riêng kế thừa từ `ValidationAttribute`

### 3. Model State

- **Centralized Validation**: Object `ModelState` theo dõi trạng thái validation của model sau khi binding
- **ModelState.IsValid**: Thuộc tính boolean cho biết model có hợp lệ hay có lỗi
- **ModelState.AddModelError**: Thêm custom error messages vào ModelState

### 4. Attributes: Tinh Chỉnh Model Binding và Validation

- `[FromQuery]`: Bind parameters từ query string
- `[FromRoute]`: Bind parameters từ route data (URL segments)
- `[FromBody]`: Bind complex objects từ request body (JSON, XML)
- `[FromHeader]`: Bind parameters từ HTTP headers
- `[Bind]`: Chỉ định rõ ràng properties nào được bind
- `[BindNever]`: Loại trừ properties khỏi binding (ngăn overposting)

### 5. Các Khái Niệm Bổ Sung

- **Default Model Binder**: Hiểu cách default model binding hoạt động và khi nào cần customization
- **Input Formatters**: Biết cách input formatters deserialize request bodies (JSON, XML)
- **Collection Binding**: Bind collections (lists, arrays) sử dụng naming conventions đúng
- **Error Handling**: Luôn kiểm tra `ModelState.IsValid` và xử lý invalid states một cách thích hợp
- **Security**: Ưu tiên bảo mật bằng cách validate và sanitize input data để ngăn chặn tấn công

---

