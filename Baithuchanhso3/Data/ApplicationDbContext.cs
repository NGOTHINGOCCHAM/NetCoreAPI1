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

       
       
    }
}
