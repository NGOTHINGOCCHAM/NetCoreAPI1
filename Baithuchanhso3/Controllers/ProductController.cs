using Microsoft.AspNetCore.Mvc;
using BaiThucHanhSo3.Data;
using BaiThucHanhSo3.Models;

namespace BaiThucHanhSo3.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context) => _context = context;

        public IActionResult Index()
        {
            var list = _context.Products.OrderBy(p => p.ProductName).ToList();
            return View(list);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid) return View(product);
            _context.Products.Add(product);
            _context.SaveChanges();
            TempData["Success"] = "Thêm sản phẩm thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return RedirectToAction("NotFoundPage", "Home");
            return View(product);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid) return View(product);
            _context.Products.Update(product);
            _context.SaveChanges();
            TempData["Success"] = "Cập nhật sản phẩm thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return RedirectToAction("NotFoundPage", "Home");
            return View(product);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return RedirectToAction("NotFoundPage", "Home");
            _context.Products.Remove(product);
            _context.SaveChanges();
            TempData["Success"] = "Xóa sản phẩm thành công!";
            return RedirectToAction("Index");
        }
    }
}
