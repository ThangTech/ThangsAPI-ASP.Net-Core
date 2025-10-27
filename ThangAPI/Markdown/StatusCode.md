# HTTP Status Codes

## Giới thiệu
HTTP Status Codes là các mã trạng thái được máy chủ trả về để thông báo kết quả của yêu cầu HTTP từ client.

## Các loại Status Code

### 2xx - Success (Thành công)
Nhóm này cho biết yêu cầu đã được nhận, hiểu và xử lý thành công.

| Status Code | Tên | Mô tả |
|-------------|-----|-------|
| 200 | OK | Yêu cầu đã thành công và máy chủ trả về dữ liệu mong muốn |
| 201 | Created | Tài nguyên đã được tạo thành công (thường dùng cho POST) |
| 204 | No Content | Yêu cầu thành công nhưng không có nội dung để trả về |

### 4xx - Client Error (Lỗi phía client)
Nhóm này cho biết lỗi xảy ra do phía client gửi yêu cầu không hợp lệ.

| Status Code | Tên | Mô tả |
|-------------|-----|-------|
| 400 | Bad Request | Yêu cầu không hợp lệ hoặc thiếu thông tin bắt buộc |
| 401 | Unauthorized | Yêu cầu chưa được xác thực (thiếu hoặc sai token) |
| 403 | Forbidden | Yêu cầu bị từ chối do không có quyền truy cập |
| 404 | Not Found | Tài nguyên không tồn tại hoặc đường dẫn sai |

### 5xx - Server Error (Lỗi phía server)
Nhóm này cho biết lỗi xảy ra do phía máy chủ không thể xử lý yêu cầu.

| Status Code | Tên | Mô tả |
|-------------|-----|-------|
| 500 | Internal Server Error | Lỗi máy chủ nội bộ, không xác định được nguyên nhân |
| 502 | Bad Gateway | Máy chủ nhận phản hồi không hợp lệ từ máy chủ upstream |
| 503 | Service Unavailable | Dịch vụ tạm thời không khả dụng (bảo trì hoặc quá tải) |
| 504 | Gateway Timeout | Máy chủ không nhận được phản hồi kịp thời từ upstream |

## Lưu ý
- Status code giúp client hiểu rõ kết quả của yêu cầu
- Nên sử dụng đúng status code để API tuân thủ chuẩn RESTful
- Kết hợp với message body để cung cấp thông tin chi tiết hơn
