# NT106-QuanLyCuaHang

Đồ án NT106 - Ứng dụng quản lý cửa hàng

## Cấu trúc thư mục

```text
NT106-QuanLyCuaHang/
├── client/                          # Ứng dụng C# WinForms
│   ├── GUI/                         # Giao diện
│   │   ├── Auth/                    # Đăng nhập, đăng ký
│   │   ├── FrontOffice/             # Giao diện Khách hàng
│   │   ├── BackOffice/              # Giao diện Nhân viên/Quản lý
│   │   └── Shared/                  # Thành phần dùng chung
│   ├── DTO/                         # Class nhận/gửi JSON
│   ├── Services/                    # Gọi REST API và TCP Server
│   └── Helpers/                     # Hàm tiện ích
│
├── backend/                         # Backend PHP / WordPress
│   └── retail-core/
│       ├── retail-core.php          # File chính của plugin
│       ├── README.md                # Tài liệu plugin
│       └── includes/
│           ├── routes/             # Khai báo REST API
│           ├── controllers/        # Xử lý request, trả JSON
│           ├── models/             # Truy vấn dữ liệu
│           ├── database/           # Tạo/migrate bảng
│           ├── middlewares/        # Auth, phân quyền
│           └── helpers/            # Hàm PHP dùng chung
│
├── socket-server/                   # C# TCP Server
│   └── Server/
│       ├── Clients/                # Quản lý client kết nối
│       ├── Handlers/               # Xử lý message/event
│       ├── Models/                 # Object message/event
│       └── Services/               # Logic realtime
│
├── database/                        # Thiết kế cơ sở dữ liệu
│   ├── ERD/                         # Sơ đồ ERD
│   ├── schema.sql                   # Cấu trúc bảng
│   ├── sample-data.sql              # Dữ liệu mẫu
│   └── README.md
│
├── docs/                            # Tài liệu đồ án
│   ├── architecture/                # Kiến trúc hệ thống
│   ├── use-case/                    # Use Case
│   ├── api/                         # API docs + JSON mẫu
│   ├── ui/                          # Mockup / ảnh giao diện
│   └── diagrams/                    # File draw.io
│
├── .gitignore
└── README.md
```

## REST API

Backend cung cấp REST API thông qua plugin **Retail Core** trên WordPress.
Ứng dụng C# WinForms sử dụng API để xác thực người dùng và thực hiện
các chức năng của hệ thống.

### Thông tin chung

| Thuộc tính | Giá trị |
|---|---|
| Base URL | `http://retail-management.test/wp-json/retail/v1` |
| API version | `v1` |
| Định dạng dữ liệu | JSON |
| Cơ chế xác thực | Bearer JWT |
| Thuật toán ký token | HS256 |
| Thời hạn access token | 3.600 giây (1 giờ) |

`http://retail-management.test` là địa chỉ website WordPress, bao gồm thư mục cài đặt
nếu có. Các endpoint bên dưới được viết tương đối so với Base URL.

**Header cho request có JSON body:**

```http
Content-Type: application/json
```

**Header cho endpoint yêu cầu xác thực:**

```http
Authorization: Bearer <access_token>
```

Các giá trị tài khoản, ID và token trong ví dụ chỉ mang tính minh họa.

### Quy ước phản hồi

**Phản hồi thành công**

Sử dụng HTTP status tương ứng và trường `success: true`.
Các trường dữ liệu còn lại được mô tả tại từng endpoint.

**Phản hồi lỗi**

Sử dụng cấu trúc lỗi của WordPress REST API:

```json
{
  "code": "invalid_credentials",
  "message": "Ten dang nhap hoac mat khau khong dung",
  "data": {
    "status": 401
  }
}
```

| Trường | Kiểu dữ liệu | Mô tả |
|---|---|---|
| `code` | string | Mã lỗi dùng để xác định cách xử lý |
| `message` | string | Thông báo mô tả lỗi |
| `data.status` | integer | HTTP status tương ứng |

Client xử lý lỗi dựa trên HTTP status và `code`, không phụ thuộc vào
nội dung chính xác của `message`.

### Authentication

Nhóm API quản lý đăng ký, đăng nhập, thông tin tài khoản, đăng xuất
và khôi phục mật khẩu.

| Method | Endpoint | Chức năng | Yêu cầu xác thực |
|---|---|---|---|
| POST | `/register` | Đăng ký tài khoản khách hàng | Không |
| POST | `/login` | Đăng nhập và cấp access token | Không |
| GET | `/me` | Lấy thông tin tài khoản hiện tại | Bearer JWT |
| POST | `/logout` | Đăng xuất và thu hồi token | Bearer JWT |
| POST | `/forgot-password` | Yêu cầu khôi phục mật khẩu qua email | Không |
| POST | `/reset-password` | Đặt lại mật khẩu bằng mã khôi phục | Mã khôi phục trong body |

#### Đăng ký tài khoản

`POST /register`

Tạo tài khoản mới với vai trò `retail_customer`.
Vai trò được gán bởi server; client không được lựa chọn vai trò
thông qua endpoint này.

**Request body**

| Trường | Kiểu | Bắt buộc | Mô tả |
|---|---|---|---|
| `username` | string | Có | Tên đăng nhập hợp lệ theo quy tắc WordPress |
| `email` | string | Có | Địa chỉ email hợp lệ |
| `password` | string | Có | Mật khẩu dài tối thiểu 8 byte |

Khoảng trắng đầu/cuối của `username` và `email` được loại bỏ.
Tên đăng nhập và email được WordPress kiểm tra khi tạo tài khoản,
bao gồm kiểm tra trùng lặp.

```json
{
  "username": "customer_demo",
  "email": "customer@example.com",
  "password": "DemoPass123!"
}
```

**Response — `201 Created`**

```json
{
  "success": true,
  "user_id": 12
}
```

| Trường | Kiểu | Mô tả |
|---|---|---|
| `success` | boolean | Kết quả xử lý |
| `user_id` | integer | ID tài khoản vừa được tạo |

**Lỗi**

| HTTP status | Code | Mô tả |
|---|---|---|
| 400 | `invalid_input` | Thiếu trường, sai kiểu dữ liệu hoặc thông tin đăng ký không hợp lệ |
| 400 | `registration_failed` | Không thể tạo tài khoản, ví dụ tên đăng nhập hoặc email đã tồn tại |

Đăng ký thành công không tự đăng nhập. Client cần gọi `/login`
để nhận access token.

#### Đăng nhập

`POST /login`

Xác thực thông tin tài khoản thông qua WordPress và cấp JWT
để sử dụng với các endpoint yêu cầu xác thực.

**Request body**

| Trường | Kiểu | Bắt buộc | Mô tả |
|---|---|---|---|
| `username` | string | Có | Định danh đăng nhập được WordPress hỗ trợ; không được rỗng sau khi loại bỏ khoảng trắng đầu/cuối |
| `password` | string | Có | Mật khẩu tài khoản; không được rỗng |

```json
{
  "username": "customer_demo",
  "password": "DemoPass123!"
}
```

**Response — `200 OK`**

```json
{
  "success": true,
  "user_id": 12,
  "access_token": "<access_token>",
  "token_type": "Bearer",
  "expires_in": 3600
}
```

| Trường | Kiểu | Mô tả |
|---|---|---|
| `success` | boolean | Kết quả xử lý |
| `user_id` | integer | ID tài khoản đã xác thực |
| `access_token` | string | JWT dùng để gọi API |
| `token_type` | string | Loại token, có giá trị `Bearer` |
| `expires_in` | integer | Thời gian hiệu lực, tính bằng giây |

**Lỗi**

| HTTP status | Code | Mô tả |
|---|---|---|
| 400 | `invalid_input` | Dữ liệu đăng nhập thiếu, rỗng hoặc sai kiểu |
| 401 | `invalid_credentials` | Thông tin đăng nhập không chính xác |
| 500 | `jwt_config_error` | Máy chủ chưa cấu hình khóa ký JWT hợp lệ |

Token hết hạn sau 1 giờ. Hiện chưa cung cấp API refresh token;
người dùng cần đăng nhập lại để nhận token mới.

#### Lấy thông tin tài khoản hiện tại

`GET /me`

Trả về thông tin của tài khoản được xác định từ JWT.
Client không cần gửi ID người dùng.

**Xác thực:** Bearer JWT.

**Request headers**

```http
Authorization: Bearer <access_token>
```

Không có request body.

**Response — `200 OK`**

```json
{
  "success": true,
  "user": {
    "id": 12,
    "username": "customer_demo",
    "email": "customer@example.com",
    "roles": [
      "retail_customer"
    ]
  }
}
```

| Trường | Kiểu | Mô tả |
|---|---|---|
| `user.id` | integer | ID tài khoản |
| `user.username` | string | Tên đăng nhập |
| `user.email` | string | Email tài khoản |
| `user.roles` | array of strings | Danh sách vai trò hiện tại |

**Lỗi:** áp dụng bảng lỗi xác thực dùng chung bên dưới.

#### Đăng xuất

`POST /logout`

Thu hồi toàn bộ JWT đã cấp trước đó cho tài khoản đang đăng nhập.
Các token cũ trên thiết bị khác cũng mất hiệu lực.

**Xác thực:** Bearer JWT.

**Request headers**

```http
Authorization: Bearer <access_token>
```

Không có request body.

**Response — `200 OK`**

```json
{
  "success": true,
  "message": "Da dang xuat"
}
```

Sau khi thành công, client xóa token đang lưu và chuyển người dùng
về màn hình đăng nhập. Token cũ tiếp tục gọi API được bảo vệ
sẽ bị từ chối.

**Lỗi:** áp dụng bảng lỗi xác thực dùng chung bên dưới.

#### Yêu cầu khôi phục mật khẩu

`POST /forgot-password`

Yêu cầu gửi hướng dẫn khôi phục tới email của tài khoản.
Email chứa tên đăng nhập và mã khôi phục để sử dụng tại
endpoint `/reset-password`.

**Request body**

| Trường | Kiểu | Bắt buộc | Mô tả |
|---|---|---|---|
| `email` | string | Có | Email của tài khoản cần khôi phục |

```json
{
  "email": "customer@example.com"
}
```

**Response — `200 OK`**

```json
{
  "success": true,
  "message": "Neu email ton tai, huong dan khoi phuc se duoc gui"
}
```

**Lỗi**

| HTTP status | Code | Mô tả |
|---|---|---|
| 400 | `invalid_input` | Email bị thiếu, sai kiểu hoặc không đúng định dạng |

**Hành vi cần lưu ý**

- Với email đúng định dạng, phản hồi không tiết lộ tài khoản
  có tồn tại trong hệ thống hay không.
- Hệ thống giới hạn gửi lại trong khoảng 60 giây cho cùng một email.
  Request trong khoảng này vẫn nhận phản hồi chung như trên.
- `200 OK` không xác nhận email đã được gửi hoặc đã đến hộp thư.
- Chức năng yêu cầu WordPress được cấu hình gửi email.

#### Đặt lại mật khẩu

`POST /reset-password`

Xác minh mã khôi phục với tài khoản tương ứng và cập nhật mật khẩu mới.
Endpoint không yêu cầu JWT; việc xác minh dựa trên `username` và `key`.

**Request body**

| Trường | Kiểu | Bắt buộc | Mô tả |
|---|---|---|---|
| `username` | string | Có | Tên đăng nhập được cung cấp trong email khôi phục |
| `key` | string | Có | Mã khôi phục nhận qua email |
| `new_password` | string | Có | Mật khẩu mới dài tối thiểu 8 byte |

```json
{
  "username": "customer_demo",
  "key": "<reset_key>",
  "new_password": "NewDemoPass456!"
}
```

**Response — `200 OK`**

```json
{
  "success": true,
  "message": "Da dat lai mat khau"
}
```

**Lỗi**

| HTTP status | Code | Mô tả |
|---|---|---|
| 400 | `invalid_input` | Thiếu trường, sai kiểu hoặc dữ liệu không đạt yêu cầu |
| 400 | `invalid_reset_key` | Mã khôi phục không hợp lệ hoặc đã hết hạn |

Sau khi đặt lại mật khẩu thành công:

- Mã khôi phục đã sử dụng không còn hợp lệ.
- Các JWT cũ của tài khoản bị thu hồi.
- Người dùng cần đăng nhập lại bằng mật khẩu mới.

Thời hạn mã khôi phục được quản lý bởi WordPress.

#### Lỗi xác thực dùng chung

Bảng sau áp dụng cho các endpoint yêu cầu Bearer JWT
trong nhóm Authentication: `/me` và `/logout`.

| HTTP status | Code | Mô tả |
|---|---|---|
| 401 | `missing_token` | Thiếu header Authorization hoặc sai định dạng Bearer |
| 401 | `invalid_token` | Token không hợp lệ, hết hạn, không đạt kiểm tra claims hoặc tài khoản không tồn tại |
| 401 | `revoked_token` | Token đã bị thu hồi |
| 500 | `jwt_config_error` | Máy chủ chưa cấu hình khóa ký JWT hợp lệ |

#### Ghi chú về mật khẩu

Mã nguồn hiện kiểm tra độ dài bằng `strlen()` của PHP, tức số byte.
Với mật khẩu ASCII, 8 byte tương ứng 8 ký tự.

