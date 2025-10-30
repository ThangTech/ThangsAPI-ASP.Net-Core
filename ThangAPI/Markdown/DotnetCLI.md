# Dotnet CLI - Command Line Interface

## Tại sao nên dùng Dotnet CLI?

Dotnet CLI mang lại nhiều lợi ích quan trọng cho các nhà phát triển .NET:

1. **Chạy trên nhiều nền tảng**: Dotnet CLI hỗ trợ Windows, macOS và Linux, giúp bạn phát triển ứng dụng trên bất kỳ hệ điều hành nào.

2. **Dễ sử dụng**: Giao diện dòng lệnh đơn giản và dễ hiểu, giúp bạn nhanh chóng thực hiện các tác vụ phát triển.

3. **Tự động hóa**: Dễ dàng tích hợp vào các quy trình CI/CD để tự động hóa việc xây dựng, kiểm thử và triển khai ứng dụng.

4. **Nhất quán**: Cung cấp một cách nhất quán để quản lý dự án, gói và các tác vụ phát triển.

5. **Linh hoạt**: Hỗ trợ nhiều loại dự án như ứng dụng web, dịch vụ, thư viện và nhiều hơn nữa.

---

## Cú pháp cơ bản của Dotnet CLI

```bash
dotnet [lệnh] [tên-ứng-dụng-hoặc-dự-án] [tùy-chọn] [tham-số]
```

**Giải thích các thành phần:**

- `dotnet`: Tên chương trình thực thi
- `[lệnh]`: Hành động bạn muốn thực hiện (ví dụ: `new`, `build`, `run`, `publish`)
- `[tên-ứng-dụng-hoặc-dự-án]`: Thường là tên của file solution hoặc project bạn đang làm việc, hoặc tên file output
- `[tùy-chọn]`: Các cờ để tùy chỉnh hành vi của lệnh (ví dụ: `-o`, `-c`, `--framework`)
- `[tham-số]`: Các giá trị bổ sung cần thiết cho lệnh hoặc tùy chọn

> **💡 Mẹo:** Bạn luôn có thể gõ `dotnet --help` để xem danh sách các lệnh chính, hoặc `dotnet [lệnh] --help` để xem chi tiết về một lệnh cụ thể và các tùy chọn của nó.

---

## Các lệnh Dotnet CLI phổ biến

Dưới đây là bảng tổng hợp các lệnh CLI thường dùng nhất:

| Lệnh (.NET Command) | Chức năng Chính | Cú pháp Ví Dụ |
|---------------------|-----------------|---------------|
| `dotnet new` | Khởi tạo dự án, solution từ template | `dotnet new webapp -o MyWebApp` |
| `dotnet add reference` | Thêm tham chiếu từ project này sang project khác | `dotnet add reference ../MyLibrary/MyLibrary.csproj` |
| `dotnet add package` | Thêm gói NuGet vào project | `dotnet add package Newtonsoft.Json` |
| `dotnet restore` | Khôi phục các gói phụ thuộc cho project/solution (Thường tự động) | `dotnet restore MySolution.sln` |
| `dotnet build` | Biên dịch mã nguồn project/solution | `dotnet build -c Release` |
| `dotnet run` | Build và chạy ứng dụng | `dotnet run --project ./src/MyApp` |
| `dotnet watch` | Giám sát file nguồn, tự động build/run lại | `dotnet watch run` |
| `dotnet test` | Tìm và chạy các bài kiểm thử (unit tests) | `dotnet test --filter "Category=Unit"` |
| `dotnet publish` | Đóng gói ứng dụng sẵn sàng cho triển khai | `dotnet publish -c Release -o ./output -r win-x64` |
| `dotnet --info` | Hiển thị thông tin chi tiết về môi trường .NET SDK/Runtime | `dotnet --info` |
| `dotnet tool` | Cài đặt, quản lý, chạy các .NET Global/Local Tools | `dotnet tool install --global dotnet-ef` |

---

## Chi tiết các lệnh quan trọng

### 1. `dotnet new` - Tạo dự án mới

Khởi tạo dự án hoặc solution từ template có sẵn.

```bash
dotnet new webapp -o MyWebApp
dotnet new console -n MyConsoleApp
dotnet new sln -n MySolution
```

### 2. `dotnet build` - Biên dịch dự án

Biên dịch mã nguồn thành file thực thi hoặc thư viện.

```bash
dotnet build
dotnet build -c Release
dotnet build MySolution.sln
```

### 3. `dotnet run` - Chạy ứng dụng

Build và chạy ứng dụng trực tiếp (thường dùng trong quá trình phát triển).

```bash
dotnet run
dotnet run --project ./src/MyApp
```

### 4. `dotnet publish` - Xuất bản ứng dụng

Đóng gói ứng dụng để triển khai lên môi trường production.

```bash
dotnet publish -c Release -o ./output
dotnet publish -c Release -o ./output -r win-x64 --self-contained
```

### 5. `dotnet test` - Chạy unit tests

Tìm và thực thi các bài kiểm thử trong dự án.

```bash
dotnet test
dotnet test --filter "Category=Unit"
dotnet test --logger "trx"
```

---

## Kết luận

**Dotnet CLI** là một công cụ mạnh mẽ, không thể thiếu trong quy trình phát triển .NET hiện đại. Nó giúp:

✅ Tăng tốc độ phát triển  
✅ Tự động hóa quy trình làm việc  
✅ Làm việc nhất quán trên nhiều nền tảng  
✅ Dễ dàng tích hợp CI/CD  

