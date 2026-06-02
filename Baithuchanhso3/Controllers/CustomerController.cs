using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaiThucHanhSo3.Data;
using BaiThucHanhSo3.Models;

namespace BaiThucHanhSo3.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CustomerController(ApplicationDbContext context) => _context = context;

        public IActionResult Index()
        {
            var list = _context.Customers.OrderBy(c => c.FullName).ToList();
            return View(list);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);
            _context.Customers.Add(customer);
            _context.SaveChanges();
            TempData["Success"] = "Thêm khách hàng thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null) return RedirectToAction("NotFoundPage", "Home");
            return View(customer);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);
            _context.Customers.Update(customer);
            _context.SaveChanges();
            TempData["Success"] = "Cập nhật khách hàng thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null) return RedirectToAction("NotFoundPage", "Home");
            return View(customer);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null) return RedirectToAction("NotFoundPage", "Home");
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            TempData["Success"] = "Xóa khách hàng thành công!";
            return RedirectToAction("Index");
        }

        // Xem chi tiết đơn hàng của một khách hàng
        public IActionResult Orders(int id)
        {
            var customer = _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderDetails)
                        .ThenInclude(d => d.Product)
                .FirstOrDefault(c => c.Id == id);

            if (customer == null) return RedirectToAction("NotFoundPage", "Home");
            return View(customer);
        }
    }
}
