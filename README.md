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

### API Overview

| Property | Value |
|---|---|
| Base URL | `http://retail-management.test/wp-json/retail/v1` |
| API Version | `v1` |
| Data Format | JSON |
| Authentication | Bearer JWT |
| Signing Algorithm | HS256 |
| Access Token Lifetime | 3.600 giây (1 giờ) |

Base URL trên sử dụng địa chỉ WordPress trong môi trường local.
Thay địa chỉ này bằng URL tương ứng khi chạy trên môi trường khác.

Các endpoint bên dưới được viết tương đối so với Base URL.

**Request Headers — JSON body**

```http
Content-Type: application/json
```

**Request Headers — Bearer authentication**

```http
Authorization: Bearer <access_token>
```

Các giá trị tài khoản, ID và token trong ví dụ chỉ mang tính minh họa.

### Response Format

**Success Response**

Sử dụng HTTP status tương ứng và trường `success: true`.
Các trường dữ liệu còn lại được mô tả tại từng endpoint.

**Error Response**

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

| Field | Type | Description |
|---|---|---|
| `code` | string | Error code để client xác định cách xử lý |
| `message` | string | Error message mô tả lỗi |
| `data.status` | integer | HTTP status code tương ứng |

Client xử lý lỗi dựa trên HTTP status code và `code`, không phụ thuộc
vào nội dung chính xác của `message`.

### Authentication

Nhóm API quản lý đăng ký, đăng nhập, thông tin tài khoản, đăng xuất
và khôi phục mật khẩu.

#### Endpoint Summary

| Method | Endpoint | Description | Authentication |
|---|---|---|---|
| POST | `/register` | Đăng ký tài khoản khách hàng | None |
| POST | `/login` | Đăng nhập và cấp access token | None |
| GET | `/me` | Lấy thông tin tài khoản hiện tại | Bearer JWT |
| POST | `/logout` | Đăng xuất và thu hồi token | Bearer JWT |
| POST | `/forgot-password` | Yêu cầu khôi phục mật khẩu qua email | None |
| POST | `/reset-password` | Đặt lại mật khẩu bằng mã khôi phục | Reset key trong request body |

#### Register

`POST /register`

Tạo tài khoản mới với role `retail_customer`.
Role được gán bởi server; client không được lựa chọn role
thông qua endpoint này.

**Authentication:** None.

**Request Body**

| Field | Type | Required | Description |
|---|---|---|---|
| `username` | string | Yes | Tên đăng nhập hợp lệ theo quy tắc WordPress |
| `email` | string | Yes | Địa chỉ email hợp lệ |
| `password` | string | Yes | Mật khẩu dài tối thiểu 8 byte |

Khoảng trắng đầu/cuối của `username` và `email` được loại bỏ.
Tên đăng nhập và email được WordPress kiểm tra khi tạo tài khoản,
bao gồm kiểm tra trùng lặp.

**Request Example**

```json
{
  "username": "customer_demo",
  "email": "customer@example.com",
  "password": "DemoPass123!"
}
```

**Success Response — `201 Created`**

```json
{
  "success": true,
  "user_id": 12
}
```

| Field | Type | Description |
|---|---|---|
| `success` | boolean | Kết quả xử lý |
| `user_id` | integer | ID tài khoản vừa được tạo |

**Error Responses**

| HTTP Status | Error Code | Description |
|---|---|---|
| 400 | `invalid_input` | Thiếu field, sai kiểu dữ liệu hoặc thông tin đăng ký không hợp lệ |
| 400 | `registration_failed` | Không thể tạo tài khoản, ví dụ tên đăng nhập hoặc email đã tồn tại |

**Notes**

Đăng ký thành công không tự đăng nhập. Client cần gọi `/login`
để nhận access token.

#### Login

`POST /login`

Xác thực thông tin tài khoản thông qua WordPress và cấp JWT
để sử dụng với các endpoint yêu cầu authentication.

**Authentication:** None.

**Request Body**

| Field | Type | Required | Description |
|---|---|---|---|
| `username` | string | Yes | Định danh đăng nhập được WordPress hỗ trợ; không được rỗng sau khi loại bỏ khoảng trắng đầu/cuối |
| `password` | string | Yes | Mật khẩu tài khoản; không được rỗng |

**Request Example**

```json
{
  "username": "customer_demo",
  "password": "DemoPass123!"
}
```

**Success Response — `200 OK`**

```json
{
  "success": true,
  "user_id": 12,
  "access_token": "<access_token>",
  "token_type": "Bearer",
  "expires_in": 3600
}
```

| Field | Type | Description |
|---|---|---|
| `success` | boolean | Kết quả xử lý |
| `user_id` | integer | ID tài khoản đã xác thực |
| `access_token` | string | JWT dùng để gọi API |
| `token_type` | string | Loại token, có giá trị `Bearer` |
| `expires_in` | integer | Thời gian hiệu lực, tính bằng giây |

**Error Responses**

| HTTP Status | Error Code | Description |
|---|---|---|
| 400 | `invalid_input` | Dữ liệu đăng nhập thiếu, rỗng hoặc sai kiểu |
| 401 | `invalid_credentials` | Thông tin đăng nhập không chính xác |
| 500 | `jwt_config_error` | Server chưa cấu hình signing key hợp lệ |

**Notes**

Access token hết hạn sau 1 giờ. Hiện chưa cung cấp refresh token endpoint;
người dùng cần đăng nhập lại để nhận token mới.

#### Get Current User

`GET /me`

Trả về thông tin của tài khoản được xác định từ JWT.
Client không cần gửi user ID.

**Authentication:** Bearer JWT.

**Request Headers**

```http
Authorization: Bearer <access_token>
```

**Request Body:** None.

**Success Response — `200 OK`**

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

| Field | Type | Description |
|---|---|---|
| `user.id` | integer | ID tài khoản |
| `user.username` | string | Tên đăng nhập |
| `user.email` | string | Email tài khoản |
| `user.roles` | array of strings | Danh sách role hiện tại |

**Error Responses**

Áp dụng bảng **Common Authentication Errors** bên dưới.

#### Logout

`POST /logout`

Thu hồi toàn bộ JWT đã cấp trước đó cho tài khoản đang đăng nhập.
Các token cũ trên thiết bị khác cũng mất hiệu lực.

**Authentication:** Bearer JWT.

**Request Headers**

```http
Authorization: Bearer <access_token>
```

**Request Body:** None.

**Success Response — `200 OK`**

```json
{
  "success": true,
  "message": "Da dang xuat"
}
```

**Error Responses**

Áp dụng bảng **Common Authentication Errors** bên dưới.

**Notes**

Sau khi thành công, client xóa token đang lưu và chuyển người dùng
về màn hình đăng nhập. Token cũ tiếp tục gọi protected endpoint
sẽ bị từ chối.

#### Forgot Password

`POST /forgot-password`

Yêu cầu gửi hướng dẫn khôi phục tới email của tài khoản.
Email chứa tên đăng nhập và reset key để sử dụng tại
endpoint `/reset-password`.

**Authentication:** None.

**Request Body**

| Field | Type | Required | Description |
|---|---|---|---|
| `email` | string | Yes | Email của tài khoản cần khôi phục |

**Request Example**

```json
{
  "email": "customer@example.com"
}
```

**Success Response — `200 OK`**

```json
{
  "success": true,
  "message": "Neu email ton tai, huong dan khoi phuc se duoc gui"
}
```

**Error Responses**

| HTTP Status | Error Code | Description |
|---|---|---|
| 400 | `invalid_input` | Email bị thiếu, sai kiểu hoặc không đúng định dạng |

**Notes**

- Với email đúng định dạng, response không tiết lộ tài khoản
  có tồn tại trong hệ thống hay không.
- Hệ thống giới hạn gửi lại trong khoảng 60 giây cho cùng một email.
  Request trong khoảng này vẫn nhận response chung như trên.
- `200 OK` không xác nhận email đã được gửi hoặc đã đến hộp thư.
- Chức năng yêu cầu WordPress được cấu hình gửi email.

#### Reset Password

`POST /reset-password`

Xác minh reset key với tài khoản tương ứng và cập nhật mật khẩu mới.
Endpoint không yêu cầu JWT; việc xác minh dựa trên `username` và `key`.

**Authentication:** Reset key trong request body; không yêu cầu Bearer JWT.

**Request Body**

| Field | Type | Required | Description |
|---|---|---|---|
| `username` | string | Yes | Tên đăng nhập được cung cấp trong email khôi phục |
| `key` | string | Yes | Reset key nhận qua email |
| `new_password` | string | Yes | Mật khẩu mới dài tối thiểu 8 byte |

**Request Example**

```json
{
  "username": "customer_demo",
  "key": "<reset_key>",
  "new_password": "NewDemoPass456!"
}
```

**Success Response — `200 OK`**

```json
{
  "success": true,
  "message": "Da dat lai mat khau"
}
```

**Error Responses**

| HTTP Status | Error Code | Description |
|---|---|---|
| 400 | `invalid_input` | Thiếu field, sai kiểu hoặc dữ liệu không đạt yêu cầu |
| 400 | `invalid_reset_key` | Reset key không hợp lệ hoặc đã hết hạn |

**Notes**

Sau khi đặt lại mật khẩu thành công:

- Reset key đã sử dụng không còn hợp lệ.
- Các JWT cũ của tài khoản bị thu hồi.
- Người dùng cần đăng nhập lại bằng mật khẩu mới.

Thời hạn reset key được quản lý bởi WordPress.

#### Common Authentication Errors

Bảng sau áp dụng cho các endpoint yêu cầu Bearer JWT
trong nhóm Authentication: `/me` và `/logout`.

| HTTP Status | Error Code | Description |
|---|---|---|
| 401 | `missing_token` | Thiếu Authorization header hoặc sai định dạng Bearer |
| 401 | `invalid_token` | Token không hợp lệ, hết hạn, không đạt kiểm tra claims hoặc tài khoản không tồn tại |
| 401 | `revoked_token` | Token đã bị thu hồi |
| 500 | `jwt_config_error` | Server chưa cấu hình signing key hợp lệ |

#### Password Validation

Mã nguồn hiện kiểm tra độ dài bằng `strlen()` của PHP, tức số byte.
Với mật khẩu ASCII, 8 byte tương ứng 8 ký tự.
