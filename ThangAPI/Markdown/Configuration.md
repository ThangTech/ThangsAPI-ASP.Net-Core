# Cấu Hình ASP.NET Core

Cấu hình là nền tảng của mọi ứng dụng, cung cấp các thiết lập và giá trị quan trọng để điều khiển hành vi của ứng dụng. Hệ thống cấu hình của ASP.NET Core rất linh hoạt, cho phép lấy dữ liệu từ nhiều nguồn khác nhau và ưu tiên theo nhu cầu.

## Khái Niệm Cơ Bản

- **Nhà cung cấp cấu hình (Configuration Providers):** Đọc dữ liệu cấu hình từ các nguồn khác nhau và tập hợp lại.
- **Nguồn cấu hình (Configuration Sources):** Nơi lưu trữ dữ liệu cấu hình (ví dụ: file, biến môi trường, tham số dòng lệnh).
- **Cặp khóa-giá trị (Key-Value Pairs):** Dữ liệu cấu hình được lưu dưới dạng cặp khóa và giá trị (chuỗi, số, boolean, ...).

## Các Nguồn Cấu Hình Phổ Biến

### 1. File (JSON, XML, INI)
- **Mục đích:** Lưu trữ cấu hình trong các file có cấu trúc. JSON là định dạng mặc định và phổ biến nhất.
- **Ưu điểm:** Dễ đọc, dễ chỉnh sửa, hỗ trợ cấu trúc phân cấp.
- **Nhược điểm:** Không nên dùng để lưu thông tin bí mật.

### 2. Biến Môi Trường (Environment Variables)
- **Mục đích:** Đọc giá trị cấu hình từ biến môi trường của hệ điều hành.
- **Ưu điểm:** Phù hợp cho các thiết lập riêng từng môi trường (ví dụ: chuỗi kết nối CSDL).
- **Nhược điểm:** Khó quản lý khi có nhiều biến hoặc cấu hình phức tạp.

### 3. Tham Số Dòng Lệnh (Command-Line Arguments)
- **Mục đích:** Ghi đè giá trị cấu hình khi chạy ứng dụng từ dòng lệnh.
- **Ưu điểm:** Linh hoạt, dễ thay đổi khi khởi động ứng dụng.
- **Nhược điểm:** Không phù hợp cho dữ liệu phức tạp hoặc nhạy cảm.

### 4. Đối Tượng Trong Bộ Nhớ (In-Memory .NET Objects)
- **Mục đích:** Lưu cấu hình trực tiếp trong mã nguồn dưới dạng dictionary hoặc object.
- **Ưu điểm:** Linh hoạt cho các cấu hình động.
- **Nhược điểm:** Không lưu trữ lâu dài, không phù hợp cho nhiều thiết lập.

### 5. Azure Key Vault
- **Mục đích:** Lưu trữ bí mật và dữ liệu cấu hình nhạy cảm một cách an toàn trên đám mây.
- **Ưu điểm:** Bảo mật cao, quản lý bí mật tập trung.
- **Nhược điểm:** Cần có đăng ký và thiết lập Azure.

### 6. Azure App Configuration
- **Mục đích:** Dịch vụ mạnh mẽ trên đám mây để quản lý cờ tính năng và thiết lập cấu hình.
- **Ưu điểm:** Quản lý cờ tính năng, cấu hình tập trung, cập nhật động.
- **Nhược điểm:** Cần có đăng ký và thiết lập Azure.

### 7. User Secrets (Phát Triển)
- **Mục đích:** Lưu trữ dữ liệu nhạy cảm (ví dụ: khóa API) trong quá trình phát triển mà không cần cam kết vào kiểm soát phiên bản.
- **Ưu điểm:** An toàn và tiện lợi cho phát triển cục bộ.
- **Nhược điểm:** Không dành cho môi trường sản xuất.

---

> **Lưu ý:** Nên sử dụng file `appsettings.json` cho cấu hình mặc định và biến môi trường cho các thông tin nhạy cảm hoặc thay đổi theo môi trường triển khai.

## Thêm và Quản Lý Nguồn Cấu Hình Trong Program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Thêm các nguồn cấu hình theo thứ tự ưu tiên mong muốn (nguồn thêm sau cùng sẽ thắng)
configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
configuration.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
configuration.AddEnvironmentVariables();
configuration.AddUserSecrets<Program>(); // Đối với bí mật trong phát triển
// ... các nguồn khác ...
```
- **Các phương thức thêm nguồn cấu hình:**
  - `AddJsonFile`: Tải cấu hình từ file JSON.
  - `AddEnvironmentVariables`: Tải cấu hình từ biến môi trường.
  - `AddUserSecrets<Program>()`: Tải cấu hình từ kho bí mật người dùng (dành cho phát triển).

## Khi Nào Sử Dụng Nguồn Cấu Hình Nào
- **appsettings.json:** Dành cho thiết lập mặc định, cấu hình cơ bản, dữ liệu không nhạy cảm.
- **appsettings.{Environment}.json:** Dành cho các ghi đè riêng theo môi trường.
- **Biến Môi Trường:** Dành cho các thiết lập riêng theo môi trường, dữ liệu nhạy cảm (chuỗi kết nối, khóa API).
- **Tham Số Dòng Lệnh:** Dành cho việc ghi đè thiết lập trong quá trình phát triển hoặc triển khai.
- **User Secrets:** Dành cho dữ liệu nhạy cảm trong phát triển cục bộ.
- **Azure Key Vault:** Dành cho việc lưu trữ bí mật và dữ liệu nhạy cảm một cách an toàn trong sản xuất.
- **Azure App Configuration:** Dành cho cập nhật cấu hình động, quản lý cờ tính năng, và quản lý tập trung.

## Thực Hành Tốt Nhất
- **Cấu Hình Tầng:** Sử dụng nhiều nguồn với thứ tự ưu tiên rõ ràng để giữ cho cấu hình của bạn có tổ chức và linh hoạt.
- **Thiết Lập Riêng Theo Môi Trường:** Tách biệt các thiết lập nhạy cảm và riêng theo môi trường vào các file thích hợp.
- **Quản Lý Bí Mật:** Sử dụng Azure Key Vault hoặc các cơ chế an toàn khác để lưu trữ dữ liệu nhạy cảm.
- **Kiểu Dữ Liệu Mạnh:** Tạo các lớp cấu hình kiểu mạnh sử dụng mẫu Options (IOptions<T>) để cải thiện độ an toàn kiểu và dễ dàng truy cập cài đặt trong mã.
- **Xác Thực:** Xác thực các giá trị cấu hình của bạn trong quá trình khởi động để phát hiện lỗi sớm.
- **Ghi Nhận:** Ghi lại các sự kiện liên quan đến cấu hình để giúp việc xử lý sự cố và gỡ lỗi.

---

## IConfiguration
Trong ASP.NET Core, giao diện `IConfiguration` là trung tâm của hệ thống cấu hình. Nó đại diện cho một tập hợp các cặp khóa-giá trị có thể được tải từ nhiều nguồn khác nhau (file JSON, biến môi trường, v.v.). Giao diện này cung cấp một cách thống nhất để truy cập các thiết lập của ứng dụng, bất kể chúng được lưu trữ ở đâu.

### Các Phương Thức, Thuộc Tính và Bộ Chỉ Mục Chính
- **GetSection(string key):** Lấy một phần cấu hình cụ thể dưới dạng `IConfigurationSection`. Các phần cho phép nhóm các thiết lập liên quan.
  ```csharp
  var connectionStrings = configuration.GetSection("ConnectionStrings");
  ```
- **GetValue<T>(string key):** Lấy một giá trị cấu hình dưới dạng kiểu đã chỉ định T.
  ```csharp
  var port = configuration.GetValue<int>("Server:Port");
  ```
- **GetConnectionString(string name):** Lấy một chuỗi kết nối từ phần "ConnectionStrings" của cấu hình.
  ```csharp
  var connectionString = configuration.GetConnectionString("DefaultConnection");
  ```
- **GetChildren():** Trả về một bộ sưu tập `IConfigurationSection` đại diện cho các phần tử con ngay lập tức của phần hiện tại.
  ```csharp
  var sections = configuration.GetSection("Logging").GetChildren();
  ```
- **Bộ chỉ mục (this[string key]):** Lấy một giá trị cấu hình dưới dạng chuỗi.
  ```csharp
  var value = configuration["Logging:LogLevel:Default"];
  ```

### Tiêm IConfiguration
- **Trong Controllers:**
  ```csharp
  public class HomeController : Controller
  {
      private readonly IConfiguration _configuration;

      public HomeController(IConfiguration configuration)
      {
          _configuration = configuration;
      }
  
      public IActionResult Index()
      {
          var myKeyValue = _configuration["MyKey"];
          return View();
      }
  }
  ```
- **Trong Services:**
  ```csharp
  public class EmailService : IEmailService
  {
      private readonly IConfiguration _configuration;

      public EmailService(IConfiguration configuration)
      {
          _configuration = configuration;
      }
  
      public void SendEmail(string to, string subject, string body)
      {
          var smtpServer = _configuration["Email:SmtpServer"];
          // ... (logic gửi email)
      }
  }
  ```
Trong cả hai trường hợp, `IConfiguration` được tiêm qua constructor bằng cách sử dụng DI của ASP.NET Core.

### Thực Hành Tốt Nhất
- **Cấu Hình Kiểu Mạnh:** Sử dụng mẫu Options (IOptions<T>) để ánh xạ các giá trị cấu hình của bạn thành các đối tượng kiểu mạnh để dễ dàng truy cập và an toàn kiểu.
- **Thiết Lập Riêng Theo Môi Trường:** Sử dụng các file appsettings.{Environment}.json để lưu trữ các giá trị cấu hình thay đổi tùy theo môi trường (Phát Triển, Sản Xuất, v.v.).
- **Quản Lý Bí Mật:** Lưu trữ thông tin nhạy cảm (ví dụ: mật khẩu, khóa API) trong Azure Key Vault hoặc các cơ chế lưu trữ an toàn khác.
- **Cấu Hình Tầng:** Kết hợp nhiều nguồn cấu hình (file, biến môi trường, v.v.) với thứ tự ưu tiên rõ ràng.
- **Tự Động Tải Lại Khi Thay Đổi:** Xem xét việc sử dụng `reloadOnChange: true` trong các nhà cung cấp cấu hình của bạn để tự động tải lại các thay đổi cấu hình mà không cần khởi động lại ứng dụng.

---

## Ví Dụ: Mẫu Options
```csharp
// MyOptions.cs
public class MyOptions
{
    public string Option1 { get; set; }
    public int Option2 { get; set; }
}

// Program.cs (hoặc Startup.cs)
builder.Services.Configure<MyOptions>(builder.Configuration.GetSection("MyOptions"));

// MyService.cs
public class MyService : IMyService
{
    private readonly IOptions<MyOptions> _options;

    public MyService(IOptions<MyOptions> options)
    {
        _options = options;
    }
 
    public void DoSomething()
    {
        var option1Value = _options.Value.Option1;
        // ...
    }
}
```
Trong ví dụ này, lớp `MyOptions` đại diện cho một phần của cấu hình. Giao diện `IOptions<MyOptions>` cung cấp một cách tiếp cận kiểu mạnh để truy cập các thiết lập đó trong các dịch vụ của bạn.

Bằng cách làm theo các thực hành tốt nhất này và tận dụng sức mạnh của `IConfiguration`, bạn có thể xây dựng các ứng dụng ASP.NET Core mạnh mẽ và linh hoạt với các thiết lập cấu hình được tổ chức tốt và dễ dàng quản lý.

---

## Cấu Hình Phân Cấp
Trong ASP.NET Core, bạn có thể tổ chức các thiết lập cấu hình của mình thành một cấu trúc phân cấp bằng cách sử dụng file JSON, XML hoặc INI. Cấu trúc phân cấp này cho phép bạn nhóm các thiết lập liên quan dưới các phần và tiểu phần, làm cho cấu hình của bạn dễ đọc, dễ bảo trì và mở rộng.

### Cấu Hình Phân Cấp Dựa Trên JSON (appsettings.json):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Inventory": {
    "StockAlertThreshold": 20,
    "WarehouseLocations": [
      "New York",
      "London",
      "Tokyo"
    ]
  }
}
```
Trong ví dụ này:
- **Các phần (Sections):** Các khóa cấp cao nhất (ConnectionStrings, Logging, Inventory) xác định các phần trong cấu hình.
- **Các phần lồng nhau (Nested Sections):** Phần Logging chứa một phần lồng nhau là LogLevel.
- **Mảng (Arrays):** Thiết lập WarehouseLocations là một mảng các chuỗi trong phần Inventory.

### Truy Cập Cấu Hình Phân Cấp Với IConfiguration
Giao diện `IConfiguration` cung cấp các phương thức để dễ dàng điều hướng và truy xuất giá trị từ cấu trúc phân cấp này.
- **GetSection(string key):** Trả về một đối tượng `IConfigurationSection` đại diện cho phần được chỉ định.
  - Sử dụng để đi sâu vào các phần lồng nhau.
- **GetValue<T>(string key):** Lấy một giá trị cấu hình dưới dạng kiểu đã chỉ định T.
  - Khóa có thể bao gồm toàn bộ đường dẫn đến giá trị, sử dụng dấu hai chấm (:) để phân tách các phần.
- **Bộ chỉ mục (this[string key]):** Lấy một giá trị cấu hình dưới dạng chuỗi.
  - Hoạt động giống như phương thức `GetValue<string>()`.

### Ví Dụ
```csharp
var connectionString = _configuration.GetConnectionString("DefaultConnection");

var logLevel = _configuration.GetValue<string>("Logging:LogLevel:Default");

// Sử dụng IConfigurationSection:
var inventorySection = _configuration.GetSection("Inventory");
var stockAlertThreshold = inventorySection.GetValue<int>("StockAlertThreshold");

// Lấy một mảng
var warehouseLocations = inventorySection.GetSection("WarehouseLocations").Get<string[]>();
```

### Thực Hành Tốt Nhất
- **Cấu Trúc Rõ Ràng:** Tổ chức các thiết lập của bạn thành các phần và tiểu phần hợp lý để cải thiện khả năng đọc và bảo trì.
- **Đặt Tên Nhất Quán:** Sử dụng quy tắc đặt tên có ý nghĩa và nhất quán cho các khóa cấu hình của bạn.
- **Kiểu Dữ Liệu Mạnh Với Mẫu Options:** Sử dụng mẫu Options (IOptions<T>) để ánh xạ các phần cấu hình của bạn thành các lớp kiểu mạnh, điều này cung cấp độ an toàn kiểu và làm cho mã của bạn dễ làm việc hơn.
- **Biến Môi Trường:** Xem xét việc sử dụng biến môi trường cho các thiết lập có thể thay đổi giữa các môi trường (ví dụ: ASPNETCORE_ENVIRONMENT).
- **Quản Lý Bí Mật:** Không bao giờ lưu trữ thông tin nhạy cảm (mật khẩu, khóa API) trực tiếp trong các file cấu hình. Sử dụng Azure Key Vault, Secret Manager, hoặc các cơ chế an toàn khác để quản lý bí mật.

---

## Ví Dụ: Mẫu Options Với Cấu Hình Phân Cấp
```csharp
// InventoryOptions.cs
public class InventoryOptions
{
    public int StockAlertThreshold { get; set; }
    public string[] WarehouseLocations { get; set; }
}

// Program.cs (hoặc Startup.cs)
builder.Services.Configure<InventoryOptions>(builder.Configuration.GetSection("Inventory"));

// Trong dịch vụ hoặc controller của bạn
public class InventoryService : IInventoryService
{
    private readonly InventoryOptions _options;

    public InventoryService(IOptions<InventoryOptions> options)
    {
        _options = options.Value;
    }
 
    // ... sử dụng _options.StockAlertThreshold và _options.WarehouseLocations
}
```

---

## Mẫu Options
Mẫu Options là một mẫu thiết kế trong ASP.NET Core cho phép bạn truy cập các giá trị cấu hình theo cách kiểu mạnh. Thay vì lấy các giá trị cấu hình dưới dạng chuỗi và chuyển đổi chúng sang các kiểu thích hợp, bạn định nghĩa các lớp POCO (Plain Old CLR Object) đại diện cho cấu trúc của các phần cấu hình. Các lớp này, được gọi là "options" classes, làm cho mã cấu hình của bạn dễ đọc, dễ bảo trì và ít lỗi hơn.

### Lợi Ích Của Mẫu Options
- **Truy Cập Kiểu Mạnh:** Truy cập các giá trị cấu hình trực tiếp dưới dạng thuộc tính của các lớp options, loại bỏ nhu cầu chuyển đổi kiểu thủ công và giảm nguy cơ lỗi thời gian chạy.
- **Hỗ Trợ IntelliSense:** Nhận hoàn thành mã và kiểm tra kiểu trong IDE khi làm việc với các thiết lập cấu hình của bạn.
- **Xác Thực:** Bạn có thể dễ dàng thêm logic xác thực vào các lớp options của mình để đảm bảo rằng các giá trị cấu hình là hợp lệ.
- **Tách Biệt Rõ Ràng:** Giữ các thiết lập cấu hình tách biệt với logic nghiệp vụ của bạn, cải thiện tổ chức tổng thể của mã.

### Khi Nào Sử Dụng Mẫu Options
- **Các Thiết Lập Liên Quan:** Khi bạn có các nhóm thiết lập cấu hình liên quan với nhau (ví dụ: thiết lập kết nối cơ sở dữ liệu, thiết lập email, cờ tính năng).
- **Truy Cập Kiểu Mạnh:** Khi bạn muốn làm việc với các giá trị cấu hình của mình theo cách an toàn về kiểu.
- **Xác Thực:** Khi bạn muốn thêm logic xác thực để đảm bảo các giá trị cấu hình của bạn là hợp lệ.

### Cách Thực Hiện Mẫu Options
1. **Tạo Lớp Options:** Định nghĩa một lớp phản ánh cấu trúc của phần cấu hình. Đảm bảo rằng tên thuộc tính khớp với các khóa trong file cấu hình của bạn.
   ```csharp
   public class EmailOptions
   {
       public string SmtpServer { get; set; } = string.Empty;
       public int SmtpPort { get; set; } = 25;
       public string SenderEmail { get; set; } = string.Empty;
       public string SenderPassword { get; set; } = string.Empty;
   }
   ```
2. **Đăng Ký Options:** Trong `Program.cs` (hoặc `Startup.cs` trong các phiên bản cũ hơn), đăng ký lớp options của bạn bằng cách sử dụng phương thức `Configure<T>` trên `IServiceCollection`:
   ```csharp
   builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
   ```
   Điều này cho DI container biết ánh xạ các thiết lập trong phần Email của cấu hình thành một thể hiện của `EmailOptions`.

3. **Tiêm IOptions<T>:** Tiêm giao diện `IOptions<T>` vào các controller hoặc dịch vụ của bạn để truy cập các options đã được ánh xạ:
   ```csharp
   public class EmailService : IEmailService
   {
       private readonly EmailOptions _options;

       public EmailService(IOptions<EmailOptions> options)
       {
           _options = options.Value; 
       }
 
       // ... sử dụng _options.SmtpServer, _options.SmtpPort, v.v. ...
   }
   ```

### Các Phương Thức Liên Quan Đến Truy Cập Cấu Hình
- **ConfigurationBinder.Get<T>(IConfiguration configuration):** Ánh xạ và trả về toàn bộ phần cấu hình thành một đối tượng kiểu mạnh của loại T.
- **ConfigurationBinder.Get(IConfiguration configuration, Type type):** Ánh xạ và trả về toàn bộ phần cấu hình thành một đối tượng của loại đã chỉ định.
- **ConfigurationBinder.Bind(IConfiguration configuration, object instance):** Ánh xạ cấu hình vào một thể hiện đối tượng đã tồn tại.

### Ví Dụ: Mẫu Options Với GetSection Và Bind
```csharp
// Program.cs (hoặc Startup.cs)
var emailOptions = new EmailOptions();
builder.Configuration.GetSection("Email").Bind(emailOptions);
builder.Services.AddSingleton(emailOptions); // Thêm đối tượng đã được ánh xạ dưới dạng singleton
```

---

## Các File Cấu Hình Riêng Theo Môi Trường
ASP.NET Core cho phép bạn tạo các file cấu hình riêng cho các môi trường khác nhau. Theo quy ước, các file này được đặt tên là `appsettings.{Environment}.json`, trong đó `{Environment}` được thay thế bằng tên của môi trường (ví dụ: `appsettings.Development.json`, `appsettings.Production.json`).

### Mục Đích:
- **Các Thiết Lập Riêng Theo Môi Trường:** Các file này lưu trữ các giá trị cấu hình độc nhất cho từng môi trường. Điều này có thể bao gồm chuỗi kết nối cơ sở dữ liệu, khóa API, mức độ ghi nhật ký, hoặc cờ tính năng.
- **Tùy Chỉnh:** Bạn có thể điều chỉnh hành vi của ứng dụng cho phát triển, kiểm tra, staging và sản xuất mà không cần phải thay đổi thủ công các thiết lập cấu hình mỗi khi triển khai.

### Thứ Tự Ưu Tiên:
ASP.NET Core tải cấu hình từ nhiều nguồn, và thứ tự mà chúng được tải xác định giá trị nào có ưu tiên hơn trong trường hợp xung đột. Thứ tự chung của ưu tiên (từ cao đến thấp) là:
1. **Tham Số Dòng Lệnh:** Bất kỳ giá trị cấu hình nào được chỉ định dưới dạng tham số dòng lệnh khi bạn chạy ứng dụng (ví dụ: `dotnet run --Logging:LogLevel:Default=Debug`) sẽ ghi đè tất cả các nguồn khác.
2. **Biến Môi Trường:** Các giá trị cấu hình được thiết lập dưới dạng biến môi trường trên hệ thống của bạn có ưu tiên hơn các giá trị trong các file cấu hình. ASP.NET Core tự động ánh xạ các biến môi trường thành các khóa cấu hình theo một quy ước nhất định. Ví dụ, biến môi trường `ConnectionStrings__DefaultConnection` sẽ ánh xạ đến khóa cấu hình `ConnectionStrings:DefaultConnection`.
3. **Bí Mật Người Dùng (Chỉ Phát Triển):** Nếu bạn đang ở trong môi trường Phát Triển, các giá trị từ kho bí mật người dùng (`secrets.json`) sẽ ghi đè các giá trị từ `appsettings.json` và `appsettings.Development.json`. Điều này hữu ích để lưu trữ thông tin nhạy cảm trong quá trình phát triển.
4. **appsettings.{Environment}.json:** Nếu có, các thiết lập từ file này sẽ ghi đè các giá trị từ file `appsettings.json` cơ bản. Điều này cho phép bạn tùy chỉnh các thiết lập cho các môi trường cụ thể.
5. **appsettings.json:** Đây là file cấu hình cơ bản luôn được tải. Nó chứa các thiết lập mặc định cho ứng dụng của bạn.

### Ví Dụ: Ghi Đè Chuỗi Kết Nối
```json
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MyDatabaseDev;Trusted_Connection=True;"
  }
}

// appsettings.Production.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=myprodserver;Database=MyDatabaseProd;User Id=myuser;Password=mypassword;"
  }
}
```
Nếu biến `ASPNETCORE_ENVIRONMENT` được thiết lập là "Production", chuỗi kết nối từ `appsettings.Production.json` sẽ được sử dụng.

### Ví Dụ Mã: GetSection() và GetValue()
```csharp
var connectionString = _configuration.GetConnectionString("DefaultConnection");

var logLevel = _configuration.GetValue<string>("Logging:LogLevel:Default");
```
- `GetConnectionString("DefaultConnection")` là một phương thức tiện lợi để lấy một chuỗi kết nối cụ thể từ phần ConnectionStrings.
- `GetValue<string>()` truy xuất các giá trị từ các phần hoặc khóa cấu hình cụ thể.

### Thực Hành Tốt Nhất
- **Cấu Trúc Lôgic:** Tổ chức các thiết lập của bạn thành các phần và tiểu phần để làm cho các file cấu hình của bạn dễ đọc và hiểu.
- **Đặt Tên Nhất Quán:** Sử dụng quy tắc đặt tên nhất quán cho các khóa cấu hình của bạn (ví dụ: kebab-case, snake_case).
- **Biến Môi Trường Cho Dữ Liệu Nhạy Cảm:** Lưu trữ thông tin nhạy cảm như khóa API và chuỗi kết nối trong biến môi trường hoặc Azure Key Vault, không phải trong các file cấu hình có thể bị cam kết vào kiểm soát phiên bản.
- **Bí Mật Người Dùng Cho Phát Triển:** Sử dụng bí mật người dùng để lưu trữ dữ liệu nhạy cảm trong quá trình phát triển mà không làm lộ chúng trong kho mã nguồn.
- **Thứ Tự Quan Trọng:** Hãy chú ý đến thứ tự ưu tiên khi thêm các nguồn cấu hình. Đặt các ghi đè quan trọng hoặc cụ thể hơn ở cuối quá trình.
- **Xác Thực:** Xem xét việc xác thực cấu hình của bạn trong quá trình khởi động ứng dụng để đảm bảo rằng tất cả các thiết lập cần thiết đều có mặt và có giá trị hợp lệ.

---

## Quản Lý Bí Mật Trong ASP.NET Core
Trong thế giới phát triển web, bạn thường cần làm việc với thông tin nhạy cảm như khóa API, chuỗi kết nối cơ sở dữ liệu, hoặc mật khẩu. Việc mã hóa cứng những giá trị này trực tiếp vào mã nguồn của bạn là một rủi ro bảo mật. Đó là lý do tại sao bạn cần đến Trình Quản Lý Bí Mật.

### Trình Quản Lý Bí Mật: Kho Lưu Trữ Kỹ Thuật Số Của Bạn
Trình Quản Lý Bí Mật là một công cụ cung cấp lưu trữ và quản lý an toàn cho các bí mật của ứng dụng bạn. Nó giữ cho dữ liệu nhạy cảm của bạn ra khỏi mã nguồn và giúp quản lý và xoay vòng bí mật dễ dàng hơn mà không cần phải triển khai lại ứng dụng.

### Bí Mật Người Dùng: Giữ Bí Mật Phát Triển An Toàn
Bí Mật Người Dùng là một tính năng thân thiện với nhà phát triển của Trình Quản Lý Bí Mật được thiết kế đặc biệt cho các môi trường phát triển cục bộ. Nó cho phép bạn lưu trữ bí mật cho một dự án cụ thể trên máy cục bộ của bạn mà không cần phải cam kết chúng vào kiểm soát phiên bản, giữ cho chúng ra khỏi kho mã của bạn.

### Cách Thiết Lập Bí Mật Người Dùng Bằng Lệnh dotnet
1. **Khởi Tạo:** Nếu bạn chưa làm, hãy khởi tạo bí mật người dùng cho dự án của bạn:
   ```bash
   dotnet user-secrets init
   ```
   Lệnh này thêm thuộc tính `UserSecretsId` vào file `.csproj` của dự án, liên kết dự án với một kho bí mật người dùng.

2. **Thiết Lập Một Bí Mật:** Sử dụng lệnh `set` để lưu trữ một bí mật:
   ```bash
   dotnet user-secrets set "MySecretName" "MySecretValue"
   ```
   Thay thế `"MySecretName"` bằng khóa mong muốn và `"MySecretValue"` bằng giá trị bí mật thực tế.

3. **Liệt Kê Bí Mật (Tùy Chọn):**
   ```bash
   dotnet user-secrets list
   ```
   Lệnh này liệt kê tất cả các bí mật bạn đã lưu cho dự án.

4. **Xóa Một Bí Mật (Tùy Chọn):**
   ```bash
   dotnet user-secrets remove "MySecretName"
   ```

### Truy Cập Bí Mật Người Dùng Trong Mã Của Bạn
```csharp
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Trong Program.cs (hoặc Startup.cs):
if (builder.Environment.IsDevelopment())
{
    configuration.AddUserSecrets<Program>();
}
```
Điều này sẽ thêm một nguồn cấu hình có thể đọc bí mật người dùng, nhưng chỉ khi môi trường được thiết lập là "Development".

Sau đó, để truy cập một bí mật người dùng, bạn có thể sử dụng các kỹ thuật giống như với bất kỳ giá trị cấu hình nào khác:
```csharp
var mySecret = configuration["MySecretName"];
```

### Thực Hành Tốt Nhất Cho Quản Lý Bí Mật
- **Không Bao Giờ Mã Hóa Bí Mật:** Luôn lưu trữ thông tin nhạy cảm trong một kho lưu trữ an toàn như Trình Quản Lý Bí Mật.
- **Nguyên Tắc Ít Quyền Nhất:** Cấp cho ứng dụng của bạn quyền tối thiểu cần thiết để truy cập các bí mật.
- **Xoay Vòng Bí Mật Định Kỳ:** Thay đổi định kỳ các bí mật của bạn để giảm thiểu rủi ro bị lộ.
- **Môi Trường Tách Biệt:** Sử dụng các bí mật khác nhau cho các môi trường khác nhau (phát triển, staging, sản xuất).
- **Tự Động Hóa:** Xem xét tự động hóa quy trình xoay vòng bí mật để tăng cường bảo mật.

### Ví Dụ: Lưu Trữ Một Khóa API Dưới Dạng Bí Mật Người Dùng
1. **Khởi Tạo:** `dotnet user-secrets init`
2. **Thiết Lập Bí Mật:** `dotnet user-secrets set "StripeApiKey" "sk_test_1234567890"`

3. **Truy Cập Trong Mã (Ví Dụ):**
   ```csharp
   var stripeApiKey = configuration["StripeApiKey"];
   ```

### Lưu Ý
- **Chỉ Dành Cho Phát Triển:** Bí mật người dùng chỉ dành cho môi trường phát triển và không nên được sử dụng trong sản xuất.
- **Lưu Trữ Cục Bộ:** Bí mật người dùng được lưu trữ trong một file JSON trên máy cục bộ của bạn. Đảm bảo rằng file này được bảo vệ.

---

## Thiết Lập Giá Trị Cấu Hình Từ Biến Môi Trường
- **Linh Hoạt:** Bạn có thể thay đổi các thiết lập của ứng dụng một cách linh hoạt mà không cần phải sửa đổi mã nguồn hay các file cấu hình.
- **Bảo Mật:** Biến môi trường là một cách an toàn để lưu trữ thông tin nhạy cảm như khóa API, chuỗi kết nối, hoặc mật khẩu mà không cần nhúng chúng vào mã nguồn.
- **Môi Trường Triển Khai:** Các môi trường khác nhau (phát triển, staging, sản xuất) thường yêu cầu các giá trị cấu hình khác nhau. Biến môi trường có thể dễ dàng được thiết lập và quản lý cho từng môi trường.
- **Tự Động Hóa:** Cách tiếp cận này rất phù hợp với các kịch bản tự động hóa cho triển khai và cấu hình.

### Cách Hoạt Động
- **Tiền Tố Biến Môi Trường:** Hệ thống cấu hình của ASP.NET Core nhận diện các biến môi trường bắt đầu bằng một tiền tố cụ thể, theo mặc định là `ASPNETCORE_`. Điều này cho phép bạn đặt tên không bị xung đột với các biến khác trên hệ thống.
- **Ánh Xạ Khóa:** Phần tên biến môi trường sau tiền tố được sử dụng làm khóa cấu hình. Ví dụ, biến môi trường `ASPNETCORE_Logging__LogLevel__Default` sẽ ánh xạ đến khóa cấu hình `Logging:LogLevel:Default`. Hai dấu gạch dưới (__) được sử dụng để đại diện cho dấu hai chấm (:) trong cấu trúc phân cấp.
- **Nhà Cung Cấp Cấu Hình:** ASP.NET Core có một nhà cung cấp cấu hình tích hợp sẵn gọi là `EnvironmentVariablesConfigurationProvider` tự động đọc các biến môi trường này và thêm chúng vào hệ thống cấu hình.

### Thiết Lập Biến Môi Trường Từ Dòng Lệnh
- **PowerShell (Windows, macOS, Linux)**
  ```powershell
  $env:ASPNETCORE_MyKey = "myvalue"         # Khóa-giá trị đơn giản
  $env:ASPNETCORE_Logging__LogLevel__Default = "Debug"  # Khóa phân cấp
  ```
  Trong PowerShell, sử dụng tiền tố `$env:` để thiết lập biến môi trường trong phiên hiện tại.

- **Command Prompt (Windows)**
  ```cmd
  set ASPNETCORE_MyKey=myvalue         # Khóa-giá trị đơn giản
  set ASPNETCORE_Logging__LogLevel__Default=Debug  # Khóa phân cấp
  ```

- **Bash (macOS, Linux)**
  ```bash
  export ASPNETCORE_MyKey="myvalue"         # Khóa-giá trị đơn giản
  export ASPNETCORE_Logging__LogLevel__Default="Debug"  # Khóa phân cấp
  ```

### Ví Dụ: Thiết Lập Một Chuỗi Kết Nối CSDL
Giả sử bạn muốn thiết lập chuỗi kết nối cơ sở dữ liệu bằng một biến môi trường. Dưới đây là cách thực hiện:
1. **Thiết Lập Biến Môi Trường:**
   ```powershell
   # Trong PowerShell
   $env:ASPNETCORE_ConnectionStrings__DefaultConnection = "Server=myServer;Database=myDb;Trusted_Connection=True;"

   # Trong Command Prompt (Windows)
   set ASPNETCORE_ConnectionStrings__DefaultConnection="Server=myServer;Database=myDb;Trusted_Connection=True;"

   # Trong Bash (macOS/Linux)
   export ASPNETCORE_ConnectionStrings__DefaultConnection="Server=myServer;Database=myDb;Trusted_Connection=True;"
   ```
   Lưu ý các dấu gạch dưới đôi (__) được sử dụng để đại diện cho dấu hai chấm (:) trong đường dẫn cấu hình.

2. **Truy Cập Trong Mã:** Bạn có thể truy xuất chuỗi kết nối này trong ứng dụng ASP.NET Core của bạn như sau:
   ```csharp
   var connectionString = _configuration.GetConnectionString("DefaultConnection");
   ```

### Lưu Ý
- **Tiền Tố:** Nhớ sử dụng tiền tố `ASPNETCORE_` cho các biến môi trường của bạn.
- **Ánh Xạ Khóa:** Các dấu gạch dưới đôi (__) trong tên biến môi trường được dịch thành dấu hai chấm (:) trong khóa cấu hình.
- **Ghi Đè:** Các giá trị biến môi trường sẽ ghi đè các giá trị được thiết lập trong `appsettings.json` hoặc các file cấu hình riêng theo môi trường.
- **Dữ Liệu Nhạy Cảm:** Đây là một cách tuyệt vời để quản lý dữ liệu nhạy cảm mà không làm lộ chúng trong mã nguồn hoặc các file cấu hình.
- **Triển Khai:** Đảm bảo thiết lập các biến môi trường thích hợp trên máy chủ sản xuất trước khi triển khai ứng dụng của bạn.

---

## Cơ Chế Của Cấu Hình Biến Môi Trường
- **Tiền Tố Biến Môi Trường:** Hệ thống cấu hình của ASP.NET Core nhận diện các biến môi trường bắt đầu bằng một tiền tố cụ thể. Theo mặc định, tiền tố này là `ASPNETCORE_`. Bạn có thể tùy chỉnh tiền tố này nếu cần. Tiền tố này giúp đặt tên không bị xung đột với các biến khác trên hệ thống của bạn.
- **Ánh Xạ Khóa:** Phần tên biến môi trường sau tiền tố được sử dụng làm khóa cấu hình. Một dấu gạch dưới đôi (__) được sử dụng để đại diện cho một dấu hai chấm (:) trong cấu trúc phân cấp của cấu hình của bạn. Ví dụ:
  - Biến Môi Trường: `ASPNETCORE_Logging__LogLevel__Default`
  - Khóa Cấu Hình: `Logging:LogLevel:Default`
- **Nhà Cung Cấp Cấu Hình:** ASP.NET Core bao gồm một nhà cung cấp cấu hình tích hợp sẵn gọi là `EnvironmentVariablesConfigurationProvider`. Nhà cung cấp này tự động đọc các biến môi trường khớp với tiền tố và thêm chúng vào cấu hình của ứng dụng. Các giá trị từ biến môi trường ghi đè bất kỳ giá trị nào khớp trong `appsettings.json` hoặc các file cấu hình riêng theo môi trường.

### Thiết Lập Biến Môi Trường Từ Dòng Lệnh
- **PowerShell (Windows, macOS, Linux)**
  ```powershell
  $env:ASPNETCORE_MyKey = "myvalue"         # Khóa-giá trị đơn giản
  $env:ASPNETCORE_Logging__LogLevel__Default = "Debug"  # Khóa phân cấp
  ```

- **Command Prompt (Windows)**
  ```cmd
  set ASPNETCORE_MyKey=myvalue         # Khóa-giá trị đơn giản
  set ASPNETCORE_Logging__LogLevel__Default=Debug  # Khóa phân cấp
  ```

- **Bash (macOS, Linux)**
  ```bash
  export ASPNETCORE_MyKey="myvalue"         # Khóa-giá trị đơn giản
  export ASPNETCORE_Logging__LogLevel__Default="Debug"  # Khóa phân cấp
  ```

### Ví Dụ: Thiết Lập Một Chuỗi Kết Nối CSDL
Giả sử bạn muốn thiết lập chuỗi kết nối cơ sở dữ liệu bằng một biến môi trường. Dưới đây là cách thực hiện:
1. **Thiết Lập Biến Môi Trường:**
   ```powershell
   # Trong PowerShell hoặc Bash
   $env:ASPNETCORE_ConnectionStrings__DefaultConnection = "Server=myServer;Database=myDb;User Id=myuser;Password=mypassword;"

   # Trong Command Prompt (Windows)
   set ASPNETCORE_ConnectionStrings__DefaultConnection="Server=myServer;Database=myDb;User Id=myuser;Password=mypassword;"
   ```
   Truy cập trong mã của bạn như bình thường:
   ```csharp
   var connectionString = _configuration.GetConnectionString("DefaultConnection");
   ```

### Các Xem Xét Quan Trọng
- **Tùy Chỉnh Tiền Tố:** Bạn có thể thay đổi tiền tố mặc định `ASPNETCORE_` bằng cách sử dụng phương thức `AddEnvironmentVariables`. Ví dụ, `configuration.AddEnvironmentVariables("CUSTOM_PREFIX_");`.
- **Nhạy Cảm Với Chữ Hoa:** Trên Linux và macOS, tên biến môi trường phân biệt chữ hoa chữ thường.
- **Triển Khai:** Khi triển khai ứng dụng của bạn, hãy đảm bảo rằng các biến môi trường thích hợp đã được thiết lập trên máy chủ đích.
- **Bảo Mật:** Mặc dù biến môi trường an toàn hơn so với việc mã hóa cứng giá trị, nhưng chúng có thể không phù hợp cho các bí mật cực kỳ nhạy cảm. Trong những trường hợp đó, hãy xem xét việc sử dụng một giải pháp quản lý bí mật chuyên dụng như Azure Key Vault hoặc HashiCorp Vault.

---

## Các File JSON Tùy Chỉnh
Trong khi ASP.NET Core hỗ trợ natively `appsettings.json` và các biến thể riêng theo môi trường, có những kịch bản mà việc sử dụng các file JSON tùy chỉnh cho cấu hình có thể có lợi:

- **Tính Modular:** Bạn có thể tổ chức các thiết lập thành nhiều file dựa trên các lĩnh vực hoặc thành phần chức năng, làm cho cấu hình của bạn dễ quản lý và dễ điều hướng hơn.
- **Tùy Chỉnh:** Bạn có thể tải các file JSON tùy chỉnh có điều kiện, dựa trên các yêu cầu cụ thể hoặc quyết định thời gian chạy.
- **Tách Biệt Quan Điểm:** Cách tiếp cận này cho phép bạn giữ các thiết lập mặc định trong `appsettings.json` trong khi duy trì các thiết lập tùy chỉnh riêng biệt.

### Thêm Các File JSON Tùy Chỉnh Làm Nguồn Cấu Hình
1. **Tạo File:** Tạo một file JSON với các thiết lập cấu hình tùy chỉnh của bạn. Giả sử gọi là `customsettings.json`:
   ```json
   {
     "CustomSettings": {
       "APIKey": "your_api_key",
       "FeatureEnabled": true,
       "NotificationSettings": {
         "EmailEnabled": true,
         "SMSEnabled": false
       }
     }
   }
   ```
2. **Thêm Vào Cấu Hình:** Trong `Program.cs`, sử dụng phương thức `AddJsonFile` để bao gồm file JSON tùy chỉnh của bạn:
   ```csharp
   var builder = WebApplication.CreateBuilder(args);
   var configuration = builder.Configuration;

   // ... (các nguồn cấu hình khác) ...

   // Thêm file JSON tùy chỉnh:
   configuration.AddJsonFile("customsettings.json", optional: true, reloadOnChange: true);

   var app = builder.Build();
   // ... (phần còn lại của ứng dụng) ...
   ```
   - `optional: true`: Đặt thành true nếu file có thể không tồn tại (ví dụ: trong một số môi trường).
   - `reloadOnChange: true`: Bật tự động tải lại cấu hình nếu file thay đổi.

### Truy Cập Các Giá Trị Cấu Hình Tùy Chỉnh
Bạn có thể truy cập các giá trị từ file JSON tùy chỉnh của mình bằng cách sử dụng các cơ chế giống như với `appsettings.json`:
```csharp
// Tùy chọn 1: Sử dụng trực tiếp IConfiguration
var apiKey = configuration["CustomSettings:APIKey"];
var featureEnabled = configuration.GetValue<bool>("CustomSettings:FeatureEnabled");

// Tùy chọn 2: Mẫu Options
var notificationSettings = configuration.GetSection("CustomSettings:NotificationSettings").Get<NotificationSettings>();
```

### Thực Hành Tốt Nhất
- **Đặt Tên:** Chọn tên mô tả và có ý nghĩa cho các file JSON tùy chỉnh của bạn.
- **Tổ Chức:** Cấu trúc các file cấu hình tùy chỉnh của bạn với các phần và tiểu phần để nâng cao khả năng đọc và bảo trì.
- **Lớp Phủ Biến Theo Môi Trường:** Tạo các phiên bản riêng theo môi trường của các file tùy chỉnh của bạn (ví dụ: `customsettings.Development.json`) để ghi đè các thiết lập trong các môi trường khác nhau.
- **Quản Lý Bí Mật:** Lưu trữ thông tin nhạy cảm (khóa API, mật khẩu) trong một kho lưu trữ an toàn như Azure Key Vault hoặc Bí Mật Người Dùng.
- **Xử Lý Lỗi:** Xử lý các lỗi tiềm ẩn, chẳng hạn như các file cấu hình bị thiếu hoặc không hợp lệ, một cách hợp lý.
- **Kiểu Dữ Liệu Mạnh Với Options:** Rất khuyến khích sử dụng Mẫu Options cho an toàn kiểu và cấu trúc mã tốt hơn.

### Ví Dụ: Mẫu Options Với File JSON Tùy Chỉnh
```csharp
// CustomSettings.cs (Lớp Options)
public class CustomSettings
{
    public string APIKey { get; set; }
    public bool FeatureEnabled { get; set; }
    public NotificationSettings NotificationSettings { get; set; }
}

// ... (các lớp options khác nếu cần) ...

// Program.cs
builder.Services.Configure<CustomSettings>(configuration.GetSection("CustomSettings"));

// Trong controller hoặc dịch vụ của bạn
public class MyController : Controller
{
    private readonly CustomSettings _settings;

    public MyController(IOptions<CustomSettings> settings)
    {
        _settings = settings.Value;
    }
}
```

---

## HttpClient
Lớp `HttpClient` là một công cụ mạnh mẽ và linh hoạt trong hệ sinh thái .NET để tương tác với các tài nguyên dựa trên web qua giao thức HTTP. Bạn sử dụng nó để gửi các yêu cầu (GET, POST, PUT, DELETE, v.v.) đến các API và nhận các phản hồi chứa dữ liệu ở nhiều định dạng khác nhau (JSON, XML, HTML).

### Các Tính Năng Chính Của HttpClient
- **Gửi Yêu Cầu:** Tạo và gửi các yêu cầu HTTP đến bất kỳ URL nào.
- **Nhận Phản Hồi:** Xử lý phản hồi từ máy chủ (mã trạng thái, tiêu đề, nội dung thân bài).
- **Hoạt Động Không Đồng Bộ:** Được thiết kế cho lập trình không đồng bộ, cho phép ứng dụng của bạn thực hiện các tác vụ khác trong khi chờ đợi phản hồi mạng.
- **Tùy Chỉnh:** Cấu hình tiêu đề yêu cầu, thời gian chờ, xác thực, và nhiều hơn nữa.

### Sử Dụng HttpClient Trong ASP.NET Core
Trong khi bạn có thể tạo và quản lý các thể hiện `HttpClient` trực tiếp, ASP.NET Core cung cấp một cách tiếp cận mạnh mẽ hơn thông qua giao diện `IHttpClientFactory`. Nhà máy này xử lý cho bạn:
- **Quản Lý Kết Nối:** Quản lý một nhóm các kết nối HTTP, tối ưu hóa hiệu suất và ngăn ngừa tình trạng cạn kiệt socket.
- **Quản Lý Thời Gian Sống:** Đảm bảo giải phóng đúng cách các thể hiện `HttpClient` để tránh rò rỉ tài nguyên.
- **Khách Hàng Được Đặt Tên:** Cho phép bạn định nghĩa và cấu hình các khách hàng được đặt tên cho các API khác nhau, mỗi khách hàng có các thiết lập riêng (địa chỉ cơ sở, tiêu đề, v.v.).

### Tích Hợp HttpClient Với Ứng Dụng Cổ Phiếu Của Bạn
Hãy phân tích cách ứng dụng cổ phiếu của bạn sử dụng `HttpClient` và `IHttpClientFactory`:
- **FinnhubService:**
  - **Tiêm:** Constructor tiêm `IHttpClientFactory` để tạo các thể hiện `HttpClient`.
  - **Xây Dựng Yêu Cầu:** Phương thức `GetStockPriceQuote` xây dựng một đối tượng `HttpRequestMessage`, chỉ định URL (bao gồm token API Finnhub) và phương thức HTTP (GET).
  - **Gửi Yêu Cầu:** Sử dụng `httpClient.SendAsync` để gửi yêu cầu một cách không đồng bộ.
  - **Xử Lý Phản Hồi:** Đọc nội dung phản hồi dưới dạng luồng và giải mã dữ liệu JSON thành một từ điển.
  - **Xử Lý Lỗi:** Kiểm tra lỗi trong phản hồi và ném ra các ngoại lệ tương ứng.

- **HomeController:**
  - **Tiêm:** Tiêm cả `FinnhubService` và `IOptions<TradingOptions>` để cấu hình.
  - **Lấy Dữ Liệu:** Hành động `Index` gọi `_finnhubService.GetStockPriceQuote` để lấy dữ liệu cổ phiếu.
  - **Tạo Mô Hình:** Ánh xạ dữ liệu đã lấy được vào một đối tượng mô hình `Stock`.
  - **Kết Xuất Giao Diện:** Mô hình `Stock` được truyền đến giao diện để hiển thị.

### Phân Tích Mã
- **IFinnhubService:** Định nghĩa một giao diện cho dịch vụ Finnhub, cho phép các triển khai khác nhau nếu cần.
- **FinnhubService:** Triển khai giao diện và sử dụng `HttpClient` để tương tác với API Finnhub.
- **TradingOptions:** Một lớp để giữ các tùy chọn cấu hình cho biểu tượng cổ phiếu mặc định (được đọc từ `appsettings.json`).
- **Stock:** Một lớp mô hình để đại diện cho dữ liệu cổ phiếu.
- **HomeController:** Controller lấy dữ liệu cổ phiếu và kết xuất giao diện.

### Thực Hành Tốt Nhất
- **IHttpClientFactory:** Luôn sử dụng `IHttpClientFactory` thay vì tạo trực tiếp các thể hiện `HttpClient` để hưởng lợi từ việc quản lý kết nối và thời gian sống đúng cách.
- **Khách Hàng Được Đặt Tên:** Đối với nhiều API, sử dụng các khách hàng được đặt tên (`_httpClientFactory.CreateClient("name");`) để cấu hình các thiết lập khác nhau cho mỗi API.
- **Xử Lý Lỗi:** Xử lý các ngoại lệ có thể xảy ra trong quá trình gửi yêu cầu HTTP, chẳng hạn như lỗi mạng hoặc phản hồi không hợp lệ.
- **Độ Bền:** Xem xét việc sử dụng Polly hoặc các thư viện khác để triển khai các mẫu thử và bộ ngắt mạch để tăng cường độ bền trong trường hợp lỗi tạm thời.

---

## Điểm Chính Cần Nhớ
- **Mục Đích:** Cung cấp các cấu hình được đặt tên để điều chỉnh hành vi của ứng dụng cho các kịch bản khác nhau (phát triển, staging, sản xuất, v.v.).
- **Biến Môi Trường:**
  - `ASPNETCORE_ENVIRONMENT` là biến môi trường chính.
  - Giá trị của nó xác định môi trường hoạt động.

### Thiết Lập Môi Trường:
- **launchSettings.json (Phát Triển):** Được thiết lập trong phần `environmentVariables` của một hồ sơ.
- **Biến Môi Trường Hệ Thống:** Được thiết lập trực tiếp trên máy của bạn (bền vững).
- **Dòng Lệnh:** Sử dụng cờ `--environment` hoặc `-e` khi chạy ứng dụng (ví dụ: `dotnet run --environment Staging`).
- **Azure App Service:** Trong cổng Azure, dưới `Configuration > Application settings`.

### Giao Diện IWebHostEnvironment:
- Sử dụng trong mã của bạn để truy cập thông tin môi trường (ví dụ: `EnvironmentName`, `WebRootPath`).
- Tiêm vào các controller hoặc middleware của bạn:
  ```csharp
  private readonly IWebHostEnvironment _env;

  public MyController(IWebHostEnvironment env)
  {
      _env = env;
  }
  ```

### Cấu Hình Riêng Theo Môi Trường:
- Tạo các file như `appsettings.Development.json`, `appsettings.Staging.json`, v.v.
- ASP.NET Core tự động tải file thích hợp dựa trên môi trường.
- Ghi đè các thiết lập cơ bản trong `appsettings.json`.

### Cấu Hình Có Điều Kiện (Trong Program.cs):
- Sử dụng `if (app.Environment.IsDevelopment())` hoặc các phương thức tương tự để áp dụng các thiết lập hoặc middleware dựa trên môi trường.
  ```csharp
  if (app.Environment.IsDevelopment())
  {
      app.UseDeveloperExceptionPage();
  }
  ```

### Môi Trường Mặc Định:
- **Phát Triển:** Mặc định cho phát triển cục bộ.
- **Staging:** Thường được sử dụng cho kiểm tra trước sản xuất.
- **Sản Xuất:** Môi trường trực tiếp.

### Môi Trường Tùy Chỉnh:
- Bạn có thể định nghĩa và sử dụng các tên môi trường tùy chỉnh.

### Thực Hành Tốt Nhất:
- **Cấu Hình Tách Biệt:** Giữ các thiết lập riêng theo môi trường trong các file tách biệt.
- **Tùy Chỉnh Middleware:** Sử dụng các pipeline middleware khác nhau cho các môi trường khác nhau (ví dụ: chỉ bật `DeveloperExceptionPage` trong phát triển).
- **Ghi Nhận:** Điều chỉnh mức độ ghi nhật ký dựa trên môi trường.
- **Cờ Tính Năng:** Sử dụng biến môi trường để bật/tắt các tính năng.

### Mẹo Phỏng Vấn
- **Giải Thích Tại Sao:** Có thể giải thích lý do sử dụng các môi trường (cấu hình, bảo mật, linh hoạt).
- **Cấu Hình:** Cho thấy cách bạn sẽ sử dụng các file `appsettings.{Environment}.json` để quản lý các thiết lập riêng theo môi trường.
- **Middleware:** Giải thích cách bạn sẽ tùy chỉnh các pipeline middleware dựa trên môi trường.
- **Triển Khai:** Thảo luận về cách bạn sẽ thiết lập biến môi trường khi triển khai trên các máy chủ khác nhau.
