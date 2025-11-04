# HTTP Protocol - Giao thức HTTP


## Tổng quan về HTTP Protocol

**HTTP (Hypertext Transfer Protocol)** là giao thức được sử dụng để truyền tải siêu văn bản (ví dụ: HTML) qua internet.

### Đặc điểm chính:

- **Mô hình Client-Server**: HTTP hoạt động theo mô hình client-server, trong đó client (thường là trình duyệt web) gửi yêu cầu đến server, sau đó server phản hồi lại với tài nguyên được yêu cầu hoặc thông báo lỗi.

- **Giao thức phi trạng thái (Stateless Protocol)**: Mỗi yêu cầu HTTP độc lập với các yêu cầu khác; server không lưu giữ thông tin từ các yêu cầu trước đó.

### Mô hình Request/Response:

1. **Client Request**: Client gửi một yêu cầu HTTP đến server
2. **Server Response**: Server xử lý yêu cầu và gửi lại phản hồi HTTP

---

## HTTP Server

### Định nghĩa:

HTTP Server là phần mềm xử lý các yêu cầu HTTP từ client và trả về phản hồi. Nó xử lý các yêu cầu đến, thực thi logic cần thiết (ví dụ: truy cập database, tạo HTML), và trả về phản hồi phù hợp.

### Các ví dụ về HTTP Server:

- Apache HTTP Server
- Nginx
- Microsoft IIS
- **Kestrel** (sử dụng với ASP.NET Core)

### Kestrel:

- Kestrel là web server đa nền tảng được tích hợp sẵn trong ASP.NET Core
- Nhẹ, hiệu suất cao
- Phù hợp để chạy cả ứng dụng web nội bộ và công khai

---

## Luồng Request và Response với Kestrel

```mermaid
Client → Kestrel → Middleware Pipeline → Application Logic → Response
```

### Các bước:

1. **Client gửi Request**: Client (ví dụ: trình duyệt web) gửi yêu cầu HTTP đến server

2. **Kestrel nhận Request**: Kestrel nhận yêu cầu và chuyển nó qua pipeline middleware của ASP.NET Core

3. **Xử lý Request**: Các component middleware xử lý yêu cầu và cuối cùng chuyển nó đến logic xử lý yêu cầu của ứng dụng

4. **Tạo Response**: Ứng dụng tạo phản hồi HTTP và gửi lại qua pipeline middleware

5. **Kestrel gửi Response**: Kestrel gửi phản hồi HTTP về cho client

---

## Cách trình duyệt sử dụng HTTP

Trình duyệt sử dụng HTTP để yêu cầu các tài nguyên như:
- Tài liệu HTML
- Hình ảnh
- File CSS
- File JavaScript

Khi người dùng nhập URL hoặc nhấp vào link, trình duyệt gửi yêu cầu HTTP đến server, sau đó server phản hồi với tài nguyên được yêu cầu.

---

## Quan sát HTTP Requests và Responses trong Chrome Dev Tools

### Các bước:

1. **Mở Chrome Dev Tools**:
   - Nhấn `F12` hoặc `Ctrl+Shift+I` (hoặc `Cmd+Option+I` trên Mac)

2. **Chuyển đến tab Network**:
   - Click vào tab Network để xem các yêu cầu và phản hồi HTTP

3. **Kiểm tra một Request**:
   - Click vào bất kỳ request nào trong danh sách để xem thông tin chi tiết:
     - **Headers**: Xem request và response headers
     - **Preview/Response**: Xem nội dung response body
     - **Timing**: Xem chi tiết thời gian của request

---

## Định dạng HTTP Response Message

### Cấu trúc Response Message:

- **Start Line**: Chứa phiên bản HTTP, mã trạng thái và thông báo trạng thái
- **Headers**: Các cặp key-value cung cấp thông tin về response
- **Body**: Tùy chọn, chứa dữ liệu thực tế (ví dụ: HTML, JSON)

### Ví dụ:

```http
HTTP/1.1 200 OK
Content-Type: text/html
Content-Length: 137

<html>
<body>
<h1>Hello, World!</h1>
</body>
</html>
```

### Các Response Headers thường dùng:

| Header | Mô tả |
|--------|-------|
| `Content-Type` | Chỉ định loại media của tài nguyên |
| `Content-Length` | Kích thước của response body tính bằng bytes |
| `Server` | Cung cấp thông tin về server xử lý request |
| `Set-Cookie` | Thiết lập cookies để lưu trữ bởi client |
| `Cache-Control` | Chỉ thị cho cơ chế caching trong cả requests và responses |

---

## Default Response Headers trong Kestrel

- **Content-Type**: Thường mặc định là `text/html` hoặc `application/json` tùy thuộc vào nội dung được phục vụ
- **Server**: Chỉ ra phần mềm server (ví dụ: Kestrel)
- **Date**: Ngày và giờ khi response được tạo

---

## HTTP Status Codes - Mã trạng thái HTTP

### Tổng quan:

Mã trạng thái được server phát hành để đáp lại yêu cầu của client nhằm chỉ ra kết quả của yêu cầu.

### Các danh mục:

| Danh mục | Mô tả |
|----------|-------|
| **1xx Informational** | Request đã nhận, đang tiếp tục xử lý |
| **2xx Success** | Request đã được nhận, hiểu và chấp nhận thành công |
| **3xx Redirection** | Cần thực hiện thêm hành động để hoàn thành request |
| **4xx Client Error** | Request chứa cú pháp sai hoặc không thể thực hiện |
| **5xx Server Error** | Server không thực hiện được request hợp lệ |

### Các mã trạng thái phổ biến:

| Mã | Tên | Mô tả |
|----|-----|-------|
| `200` | OK | Request thành công |
| `201` | Created | Request thành công và tài nguyên mới được tạo |
| `204` | No Content | Server xử lý thành công nhưng không trả về nội dung |
| `400` | Bad Request | Server không hiểu request do cú pháp không hợp lệ |
| `401` | Unauthorized | Yêu cầu xác thực |
| `403` | Forbidden | Client không có quyền truy cập nội dung |
| `404` | Not Found | Server không tìm thấy tài nguyên được yêu cầu |
| `500` | Internal Server Error | Server gặp lỗi không mong đợi |
| `502` | Bad Gateway | Server đóng vai trò gateway nhận phản hồi không hợp lệ |
| `503` | Service Unavailable | Server chưa sẵn sàng xử lý request |

---

## Thiết lập Status Codes và Response Headers trong ASP.NET Core

### Ví dụ 1: Thiết lập Custom Headers

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) =>
{
    context.Response.Headers["MyKey"] = "my value";
    context.Response.Headers["Server"] = "My server";
    context.Response.Headers["Content-Type"] = "text/html";
    await context.Response.WriteAsync("<h1>Hello</h1>");
    await context.Response.WriteAsync("<h2>World</h2>");
});

app.Run();
```

**Giải thích:**
- `context.Response.Headers["MyKey"] = "my value";` - Thêm custom header vào response
- `context.Response.Headers["Server"] = "My server";` - Sửa đổi Server header
- `context.Response.Headers["Content-Type"] = "text/html";` - Thiết lập Content-Type header
- `await context.Response.WriteAsync("<h1>Hello</h1>");` - Ghi phần đầu của response body
- `await context.Response.WriteAsync("<h2>World</h2>");` - Ghi phần tiếp theo của response body

### Ví dụ 2: Thiết lập Status Code

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) =>
{
    if (1 == 1)
    {
        context.Response.StatusCode = 200;
    }
    else
    {
        context.Response.StatusCode = 400;
    }
    await context.Response.WriteAsync("Hello");
    await context.Response.WriteAsync(" World");
});

app.Run();
```

**Giải thích:**
- `context.Response.StatusCode = 200;` - Thiết lập status code là 200 OK
- `context.Response.StatusCode = 400;` - Thiết lập status code là 400 Bad Request (dòng này không được thực thi do điều kiện)
- `await context.Response.WriteAsync("Hello");` - Ghi phần đầu của response body
- `await context.Response.WriteAsync(" World");` - Ghi phần tiếp theo của response body

---

## HTTP Requests - Yêu cầu HTTP

Trong thế giới ứng dụng web, HTTP request là cách client nói: "Này server, tôi cần thứ gì đó". "Thứ gì đó" này có thể là một trang web, hình ảnh, dữ liệu từ database, hoặc kết quả của một số tính toán phía server. Client, thường là trình duyệt web, gửi yêu cầu này đến server, server xử lý nó và trả về phản hồi.

### Cấu trúc của HTTP Request

Một HTTP request bao gồm các phần:

#### 1. Start Line (Dòng bắt đầu)

Đây là dòng đầu tiên của request, chứa ba thông tin quan trọng:

**Method (Phương thức)**: Chỉ ra hành động mà client muốn server thực hiện:
- `GET` - Lấy dữ liệu từ server
- `POST` - Gửi dữ liệu đến server (ví dụ: dữ liệu form)
- `PUT` - Cập nhật tài nguyên hiện có trên server
- `DELETE` - Xóa tài nguyên trên server

**Request URI (Uniform Resource Identifier)**: Đường dẫn đến tài nguyên trên server mà client đang yêu cầu

**HTTP Version**: Chỉ định phiên bản của giao thức HTTP đang sử dụng (ví dụ: HTTP/1.1 hoặc HTTP/2)

#### 2. Headers (Tiêu đề)

Cung cấp thông tin bổ sung về request:

| Header | Mô tả |
|--------|-------|
| `User-Agent` | Trình duyệt hoặc ứng dụng của client |
| `Accept` | Các loại nội dung client có thể hiểu (ví dụ: HTML, JSON) |
| `Host` | Tên miền của server |
| `Content-Type` | Loại dữ liệu được gửi trong request body (nếu có) |
| `Authorization` | Thông tin xác thực (nếu yêu cầu) |

#### 3. Empty Line (Dòng trống)

Phân tách headers khỏi body của request

#### 4. Body (Thân) - Tùy chọn

Phần này chứa dữ liệu mà client đang gửi đến server. Ví dụ: POST request có thể bao gồm dữ liệu form hoặc dữ liệu JSON.

---

## Query Strings - Truyền tham số trong URL

**Query string** là cách truyền tham số đến server trong chính URL. Nó bắt đầu bằng dấu hỏi chấm (`?`) và theo sau đường dẫn trong URL. Mỗi tham số là một cặp key-value, được phân tách bằng dấu bằng (`=`), và nhiều tham số được phân tách bằng dấu và (`&`).

### Ví dụ:

```
https://example.com/products?category=electronics&brand=apple
```

Trong ví dụ này, `category=electronics` và `brand=apple` là các tham số được truyền đến server.

---

## Request Object trong ASP.NET Core

ASP.NET Core cung cấp đối tượng `HttpRequest` cho phép bạn truy cập tất cả thông tin trong một request đến. Đối tượng này có các thuộc tính như:

| Thuộc tính | Mô tả |
|------------|-------|
| `Method` | Phương thức HTTP (GET, POST, etc.) |
| `Path` | Đường dẫn URI được client yêu cầu |
| `Query` | Collection các tham số query string |
| `Headers` | Collection các request headers |
| `Body` | Stream đại diện cho request body (nếu có) |

### Code 1: Hiển thị Request Path và Method

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) =>
{
    string path = context.Request.Path;
    string method = context.Request.Method;

    context.Response.Headers["Content-type"] = "text/html";
    await context.Response.WriteAsync($"<p>{path}</p>");
    await context.Response.WriteAsync($"<p>{method}</p>");
});

app.Run();
```

Code này định nghĩa một middleware component đơn giản:
- Trích xuất `Path` và `Method` từ đối tượng Request
- Thiết lập Content-type response header là text/html
- Ghi path và method được trích xuất vào response body dưới dạng các đoạn HTML

### Code 2: Xử lý GET Requests với Query Parameters

```csharp
app.Run(async (HttpContext context) =>
{
    context.Response.Headers["Content-type"] = "text/html";
    if (context.Request.Method == "GET")
    {
        if (context.Request.Query.ContainsKey("id"))
        {
            string id = context.Request.Query["id"];
            await context.Response.WriteAsync($"<p>{id}</p>");
        }
    }
});
```

Code này tập trung vào GET requests:
- Thiết lập Content-type response header
- Kiểm tra xem request method có phải là GET không
- Nếu đúng, kiểm tra xem query parameter có tên "id" có tồn tại không
- Nếu tìm thấy, trích xuất giá trị của tham số "id" và hiển thị nó

### Code 3: Trích xuất User-Agent Header

```csharp
app.Run(async (HttpContext context) =>
{
    context.Response.Headers["Content-type"] = "text/html";
    if (context.Request.Headers.ContainsKey("User-Agent"))
    {
        string userAgent = context.Request.Headers["User-Agent"];
        await context.Response.WriteAsync($"<p>{userAgent}</p>");
    }
});
```

Code này:
- Thiết lập Content-type response header
- Kiểm tra xem User-Agent header có trong request không
- Nếu tìm thấy, trích xuất giá trị của User-Agent header và hiển thị nó, cho biết trình duyệt hoặc ứng dụng của client

---

## HTTP Methods - Các phương thức HTTP

### GET - Lấy dữ liệu

Phương thức GET chủ yếu được thiết kế để lấy dữ liệu từ server. Hãy nghĩ về nó như việc yêu cầu server một tài nguyên cụ thể, như một trang web, hình ảnh, hoặc một số dữ liệu từ database.

#### Đặc điểm của GET:

| Đặc điểm | Mô tả |
|----------|-------|
| **Dữ liệu trong URL** | Tham số được thêm vào URL dưới dạng query string, có thể nhìn thấy trong thanh địa chỉ |
| **Giới hạn kích thước** | Kích thước dữ liệu bị hạn chế do giới hạn độ dài URL |
| **Idempotent** | Thực hiện cùng một GET request nhiều lần sẽ có cùng hiệu quả |
| **Caching** | GET requests có thể được cache |
| **Bảo mật** | Kém bảo mật hơn POST vì dữ liệu hiển thị trong URL |

#### Ví dụ GET Request:

```http
GET /products?category=electronics&brand=apple HTTP/1.1
Host: example.com
```

---

### POST - Gửi dữ liệu

Phương thức POST chủ yếu được sử dụng để gửi dữ liệu đến server để xử lý. Dữ liệu này thường được bao gồm trong body của request và không hiển thị trong URL.

#### Đặc điểm của POST:

| Đặc điểm | Mô tả |
|----------|-------|
| **Dữ liệu trong Body** | Dữ liệu được gửi trong request body, phù hợp cho dữ liệu lớn hoặc nhạy cảm |
| **Không Idempotent** | Các POST request lặp lại có thể dẫn đến kết quả khác nhau |
| **Không Cache** | POST requests thường không được cache |
| **Bảo mật** | An toàn hơn GET vì dữ liệu không hiển thị trong URL |

#### Ví dụ POST Request:

```http
POST /login HTTP/1.1
Host: example.com
Content-Type: application/x-www-form-urlencoded

username=john&password=secret
```

---

### Lựa chọn giữa GET và POST

#### Sử dụng GET khi:
- ✅ Bạn đang lấy dữ liệu từ server
- ✅ Bạn muốn request có thể bookmark được
- ✅ Dữ liệu được gửi nhỏ và không nhạy cảm

#### Sử dụng POST khi:
- ✅ Bạn đang gửi dữ liệu đến server để xử lý
- ✅ Request có thể gây thay đổi trên server
- ✅ Bạn đang gửi dữ liệu nhạy cảm hoặc dữ liệu lớn

---

## Postman - Công cụ kiểm thử API

Postman là một công cụ phát triển và kiểm thử API đa năng. Nó cho phép bạn dễ dàng tạo các HTTP requests, gửi chúng đến ứng dụng ASP.NET Core (hoặc bất kỳ API nào), và kiểm tra các responses. Đây là cách tuyệt vời để debug, thử nghiệm và khám phá các API endpoints của bạn.

### Cài đặt

1. **Download**: Truy cập trang web chính thức của Postman (https://www.postman.com/downloads/) và tải phiên bản phù hợp cho hệ điều hành của bạn (Windows, macOS, Linux)

2. **Install**: Làm theo hướng dẫn trên màn hình để cài đặt Postman

### Sử dụng: Gửi Requests đến ứng dụng ASP.NET Core

Giả sử ứng dụng ASP.NET Core của bạn đang chạy local tại `https://localhost:7070` và có endpoint `/api/products`. Đây là cách sử dụng Postman:

#### Các bước:

1. **Khởi động Postman**: Mở ứng dụng Postman

2. **Tạo Request mới**:
   - Click vào nút "New" ở góc trên bên trái
   - Chọn "Request" từ các tùy chọn

3. **Thiết lập Request Method và URL**:
   - Trong request builder, chọn phương thức HTTP phù hợp (GET, POST, PUT, DELETE, etc.) từ dropdown
   - Nhập URL đầy đủ của ASP.NET Core endpoint (ví dụ: `https://localhost:7070/api/products`)

4. **(Tùy chọn) Thêm Headers**:
   - Nếu endpoint yêu cầu headers cụ thể (như Content-Type), click vào tab "Headers" và thêm chúng dưới dạng các cặp key-value

5. **(Tùy chọn) Thêm Request Body**:
   - Nếu bạn đang gửi dữ liệu với request (ví dụ: dữ liệu JSON cho POST request), click vào tab "Body"
   - Chọn định dạng (ví dụ: "raw" cho JSON) và nhập dữ liệu của bạn

6. **Gửi Request**:
   - Click nút "Send"

7. **Kiểm tra Response**:
   - Response từ ứng dụng ASP.NET Core sẽ xuất hiện ở phần dưới của Postman. Bạn sẽ thấy:
     - Status code (200 OK, 404 Not Found, etc.)
     - Response headers
     - Response body (nếu có)

---

## Tóm tắt

### HTTP (Hypertext Transfer Protocol):

- **Nền tảng của Web**: HTTP là giao thức cung cấp sức mạnh cho World Wide Web. Nó định nghĩa cách clients (trình duyệt, ứng dụng) và servers giao tiếp
- **Chu kỳ Request-Response**: Giao tiếp theo mô hình request-response. Client gửi request, và server gửi lại response
- **Phi trạng thái**: HTTP là phi trạng thái, có nghĩa là mỗi request độc lập. Servers không nhớ các tương tác trước đó
- **Methods**: Các phương thức HTTP định nghĩa hành động (GET, POST, PUT, DELETE, etc.)
- **Versions**: HTTP/1.1 và HTTP/2 là các phiên bản được sử dụng phổ biến nhất

### HTTP Requests:

- **Mục đích**: Khởi tạo giao tiếp, yêu cầu tài nguyên hoặc hành động từ server
- **Cấu trúc**: Start line (method, URI, version), headers, empty line, optional body
- **Methods**:
  - `GET`: Lấy dữ liệu, idempotent, có thể cache
  - `POST`: Gửi dữ liệu, không idempotent, thường không cache
  - `PUT`, `DELETE`: Cập nhật và xóa tài nguyên
- **Headers**: Cung cấp metadata như content type, user agent, authentication
- **Body**: Được sử dụng để gửi dữ liệu với POST, PUT, etc.

### HTTP Responses:

- **Mục đích**: Phản hồi của server cho request
- **Cấu trúc**: Start line (version, status code, reason phrase), headers, empty line, optional body
- **Status Codes**: Mã ba chữ số chỉ ra kết quả (200 OK, 404 Not Found, 500 Internal Server Error)
- **Headers**: Cung cấp metadata về response (content type, length, caching)
- **Body**: Chứa dữ liệu được yêu cầu (HTML, JSON, etc.) hoặc thông báo lỗi

---

> **Lưu ý**: Tài liệu này được thiết kế để giúp bạn hiểu rõ về HTTP Protocol và cách sử dụng nó trong ASP.NET Core. Hãy thực hành với các ví dụ code để nắm vững kiến thức!
