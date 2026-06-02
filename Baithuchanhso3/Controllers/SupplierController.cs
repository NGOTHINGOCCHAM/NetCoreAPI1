using Microsoft.AspNetCore.Mvc;
using BaiThucHanhSo3.Data;
using BaiThucHanhSo3.Models;

namespace BaiThucHanhSo3.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SupplierController(ApplicationDbContext context) => _context = context;

        public IActionResult Index()
        {
            var list = _context.Suppliers.OrderBy(s => s.SupplierName).ToList();
            return View(list);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Supplier supplier)
        {
            if (!ModelState.IsValid) return View(supplier);
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
            TempData["Success"] = "Thêm nhà cung cấp thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var s = _context.Suppliers.Find(id);
            if (s == null) return RedirectToAction("NotFoundPage", "Home");
            return View(s);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Supplier supplier)
        {
            if (!ModelState.IsValid) return View(supplier);
            _context.Suppliers.Update(supplier);
            _context.SaveChanges();
            TempData["Success"] = "Cập nhật nhà cung cấp thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var s = _context.Suppliers.Find(id);
            if (s == null) return RedirectToAction("NotFoundPage", "Home");
            return View(s);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var s = _context.Suppliers.Find(id);
            if (s == null) return RedirectToAction("NotFoundPage", "Home");
            _context.Suppliers.Remove(s);
            _context.SaveChanges();
            TempData["Success"] = "Xóa nhà cung cấp thành công!";
            return RedirectToAction("Index");
        }
    }
}
