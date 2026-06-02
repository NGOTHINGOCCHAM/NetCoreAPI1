using Microsoft.EntityFrameworkCore;
using BaiThucHanhSo3.Models;

namespace BaiThucHanhSo3.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Buổi 7-8: Student + Faculty
        public DbSet<Student>        Students        { get; set; }
        public DbSet<Faculty>        Faculties       { get; set; }

        // Buổi 9: Bán hàng
        public DbSet<Customer>       Customers       { get; set; }
        public DbSet<Product>        Products        { get; set; }
        public DbSet<Order>          Orders          { get; set; }
        public DbSet<OrderDetail>    OrderDetails    { get; set; }

        // Buổi 12: Quản lý kho thiết bị điện tử
        public DbSet<Supplier>       Suppliers       { get; set; }
        public DbSet<DeviceType>     DeviceTypes     { get; set; }
        public DbSet<Device>         Devices         { get; set; }
        public DbSet<ImportReceipt>  ImportReceipts  { get; set; }
        public DbSet<ImportDetail>   ImportDetails   { get; set; }
        public DbSet<ExportReceipt>  ExportReceipts  { get; set; }
        public DbSet<ExportDetail>   ExportDetails   { get; set; }
    }
}
