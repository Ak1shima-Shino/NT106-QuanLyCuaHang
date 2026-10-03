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
│   └── nt106-quan-ly-cua-hang/
│       ├── nt106-quan-ly-cua-hang.php   # File chính của plugin
│       └── includes/
│           ├── routes/              # Khai báo REST API
│           ├── controllers/         # Xử lý request, trả JSON
│           ├── models/              # Truy vấn dữ liệu
│           ├── database/            # Tạo/migrate bảng
│           ├── middlewares/         # Auth, phân quyền
│           └── helpers/             # Hàm PHP dùng chung
│
├── socket-server/                   # C# TCP Server
│   └── Server/
│       ├── Clients/                 # Quản lý client kết nối
│       ├── Handlers/                # Xử lý message/event
│       ├── Models/                  # Object message/event
│       └── Services/                # Logic realtime
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