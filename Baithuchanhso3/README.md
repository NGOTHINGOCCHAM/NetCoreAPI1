# Bài Thực Hành Số 3 - ASP.NET Core MVC

## Thông tin
- **Framework:** ASP.NET Core MVC (.NET 10)
- **Database:** SQL Server (Entity Framework Core - Code First)
- **Sinh viên:** [Họ tên của bạn]

---

## Cách chạy project

```bash
# 1. Clone project về
git clone <link repo>

# 2. Cập nhật connection string trong appsettings.json

# 3. Tạo database
dotnet ef database update

# 4. Chạy project
dotnet run
```

---

## Buổi 7 — CRUD Student có Validation

**Yêu cầu:** Hoàn thiện chức năng CRUD có validate dữ liệu với đối tượng Student

**Đã thực hiện:**
- ✅ Tạo Model `Student` với DataAnnotations (Required, StringLength, Range, EmailAddress)
- ✅ Tạo `ApplicationDbContext` kết nối SQL Server
- ✅ Sử dụng Migrations tạo bảng Student
- ✅ CRUD đầy đủ: Xem danh sách, Thêm, Sửa, Xoá
- ✅ Validate phía server + hiển thị lỗi bằng `asp-validation-for`
- ✅ Thông báo thành công bằng `TempData`

**Các file chính:**
```
Models/Student.cs
Data/ApplicationDbContext.cs
Controllers/StudentController.cs
Views/Student/ (Index, Create, Edit, Delete)
```

**Ảnh chụp màn hình:**

> *[Thêm ảnh chụp màn hình tại đây]*

---

## Buổi 8 — Khoá ngoại + ViewModel

**Yêu cầu:**
- Tạo class `Faculty` và `Student`, liên kết khoá ngoại qua `FacultyId` (một khoa có nhiều sinh viên, một sinh viên chỉ thuộc một khoa)
- Tạo ViewModel hiển thị: Mã sinh viên, Họ tên, Khoa

**Đã thực hiện:**
- ✅ Tạo Model `Faculty`
- ✅ Thêm `FacultyId` (khoá ngoại) vào Model `Student`
- ✅ Tạo `StudentFacultyViewModel` hiển thị dữ liệu từ 2 bảng
- ✅ Sử dụng LinQ `Include()` để join dữ liệu
- ✅ CRUD đầy đủ cho Faculty
- ✅ Dropdown chọn khoa khi thêm/sửa sinh viên
- ✅ Migration: `AddFacultyAndForeignKey`

**Các file chính:**
```
Models/Faculty.cs
Models/Student.cs                        (cập nhật thêm FacultyId)
Models/ViewModels/StudentFacultyViewModel.cs
Controllers/FacultyController.cs
Controllers/StudentController.cs         (cập nhật)
Views/Faculty/ (Index, Create, Edit, Delete)
Views/Student/ (Index, Create, Edit, Delete)
```

**Ảnh chụp màn hình:**

> *[Thêm ảnh chụp màn hình tại đây]*

---

## Buổi 9 — Quan hệ nhiều bảng + CRUD

**Yêu cầu:**
- Tạo class: Khách hàng, Đơn hàng, Chi tiết đơn hàng, Sản phẩm
- Một khách hàng có nhiều đơn hàng
- Một đơn hàng thuộc 1 khách hàng
- Một đơn hàng có nhiều sản phẩm lưu ở chi tiết đơn hàng
- Ràng buộc dữ liệu các thuộc tính trên model
- CRUD với các bảng dữ liệu trên
- Xem thông tin chi tiết đơn hàng của một khách hàng

**Đã thực hiện:**
- ✅ Tạo 4 Model: `Customer`, `Product`, `Order`, `OrderDetail`
- ✅ Quan hệ 1-nhiều: Customer → Orders, Order → OrderDetails
- ✅ Validate đầy đủ trên tất cả model
- ✅ CRUD: Customer, Product, Order
- ✅ Thêm/xoá sản phẩm trong chi tiết đơn hàng
- ✅ Tính tổng tiền tự động (`[NotMapped]`)
- ✅ Trang xem tất cả đơn hàng của một khách hàng
- ✅ Migration: `AddOrderSystem`

**Các file chính:**
```
Models/Customer.cs
Models/Product.cs
Models/Order.cs
Models/OrderDetail.cs
Controllers/CustomerController.cs
Controllers/ProductController.cs
Controllers/OrderController.cs
Views/Customer/ (Index, Create, Edit, Delete, Orders)
Views/Product/  (Index, Create, Edit, Delete)
Views/Order/    (Index, Create, Edit, Delete, Details)
```

**Ảnh chụp màn hình:**

> *[Thêm ảnh chụp màn hình tại đây]*

---

## Buổi 10 — Đọc dữ liệu từ Excel

**Yêu cầu:** Xây dựng chức năng đọc dữ liệu từ file Excel và lưu vào bảng Student

**Đã thực hiện:**
- ✅ Cài đặt package `ClosedXML` để đọc file Excel
- ✅ Tạo file Excel mẫu có thể tải về (StudentCode, FullName, Age, Email)
- ✅ Giao diện upload file Excel
- ✅ Validate từng dòng dữ liệu (trống, tuổi không hợp lệ, trùng mã SV)
- ✅ Hiển thị kết quả: số dòng thành công, số dòng bỏ qua, danh sách lỗi
- ✅ Lưu dữ liệu hợp lệ vào bảng Student

**Các file chính:**
```
BaiThucHanhSo3.csproj  (thêm ClosedXML)
Controllers/ImportController.cs
Views/Import/Index.cshtml
```

**Ảnh chụp màn hình:**

> *[Thêm ảnh chụp màn hình tại đây]*

---

## Buổi 12 — Quản lý kho thiết bị điện tử

**Yêu cầu:**
- Quản lý nhà cung cấp (CRUD)
- Quản lý loại thiết bị (CRUD + Tìm kiếm)
- Quản lý thiết bị (CRUD + Tìm kiếm)
- Quản lý nhập kho: phiếu nhập có thông tin nhiều thiết bị (đơn giá nhập, số lượng, thành tiền)
- Quản lý xuất kho: phiếu xuất có thông tin nhiều thiết bị (đơn giá xuất, số lượng, thành tiền)

**Đã thực hiện:**
- ✅ Tạo Model `Supplier` — CRUD
- ✅ Tạo Model `DeviceType` — CRUD + Tìm kiếm theo tên
- ✅ Tạo Model `Device` — CRUD + Tìm kiếm theo tên/mã + Lọc theo loại + Hiển thị tồn kho
- ✅ Tạo Model `ImportReceipt` + `ImportDetail` — Quản lý phiếu nhập kho
- ✅ Tạo Model `ExportReceipt` + `ExportDetail` — Quản lý phiếu xuất kho
- ✅ Tự động cộng tồn kho khi nhập, trừ tồn kho khi xuất
- ✅ Kiểm tra tồn kho đủ trước khi xuất
- ✅ Hoàn trả tồn kho khi xoá phiếu nhập/xuất
- ✅ Migration: `AddWarehouseSystem`

**Các file chính:**
```
Models/Supplier.cs
Models/DeviceType.cs
Models/Device.cs
Models/ImportReceipt.cs
Models/ImportDetail.cs
Models/ExportReceipt.cs
Models/ExportDetail.cs
Controllers/SupplierController.cs
Controllers/DeviceTypeController.cs
Controllers/DeviceController.cs
Controllers/ImportReceiptController.cs
Controllers/ExportReceiptController.cs
Views/Supplier/
Views/DeviceType/
Views/Device/
Views/ImportReceipt/
Views/ExportReceipt/
```

**Ảnh chụp màn hình:**

> *[Thêm ảnh chụp màn hình tại đây]*

---

## Tổng kết Migration

| Migration | Nội dung |
|-----------|----------|
| `InitCleanDB` | Tạo bảng Student ban đầu |
| `AddAgeAndEmail` | Thêm cột Age, Email vào Student |
| `AddFacultyAndForeignKey` | Thêm bảng Faculty, cột FacultyId vào Student |
| `AddOrderSystem` | Thêm bảng Customer, Product, Order, OrderDetail |
| `AddWarehouseSystem` | Thêm bảng Supplier, DeviceType, Device, ImportReceipt, ImportDetail, ExportReceipt, ExportDetail |
