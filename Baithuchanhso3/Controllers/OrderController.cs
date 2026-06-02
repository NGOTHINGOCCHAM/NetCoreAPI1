using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaiThucHanhSo3.Data;
using BaiThucHanhSo3.Models;

namespace BaiThucHanhSo3.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context) => _context = context;

        public IActionResult Index()
        {
            var list = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(_context.Customers.OrderBy(c => c.FullName).ToList(), "Id", "FullName");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CustomerId = new SelectList(_context.Customers.OrderBy(c => c.FullName).ToList(), "Id", "FullName");
                return View(order);
            }
            _context.Orders.Add(order);
            _context.SaveChanges();
            TempData["Success"] = "Tạo đơn hàng thành công! Hãy thêm sản phẩm vào đơn.";
            return RedirectToAction("Details", new { id = order.Id });
        }

        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return RedirectToAction("NotFoundPage", "Home");
            ViewBag.CustomerId = new SelectList(_context.Customers.OrderBy(c => c.FullName).ToList(), "Id", "FullName", order.CustomerId);
            return View(order);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Order order)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CustomerId = new SelectList(_context.Customers.OrderBy(c => c.FullName).ToList(), "Id", "FullName", order.CustomerId);
                return View(order);
            }
            _context.Orders.Update(order);
            _context.SaveChanges();
            TempData["Success"] = "Cập nhật đơn hàng thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Include(o => o.Customer).FirstOrDefault(o => o.Id == id);
            if (order == null) return RedirectToAction("NotFoundPage", "Home");
            return View(order);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var order = _context.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.Id == id);
            if (order == null) return RedirectToAction("NotFoundPage", "Home");
            _context.OrderDetails.RemoveRange(order.OrderDetails);
            _context.Orders.Remove(order);
            _context.SaveChanges();
            TempData["Success"] = "Xóa đơn hàng thành công!";
            return RedirectToAction("Index");
        }

        // Xem chi tiết đơn hàng + thêm sản phẩm vào đơn
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Product)
                .FirstOrDefault(o => o.Id == id);
            if (order == null) return RedirectToAction("NotFoundPage", "Home");

            ViewBag.Products = new SelectList(_context.Products.OrderBy(p => p.ProductName).ToList(), "Id", "ProductName");
            return View(order);
        }

        // Thêm sản phẩm vào chi tiết đơn hàng
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddDetail(int orderId, int productId, int quantity)
        {
            var product = _context.Products.Find(productId);
            if (product == null) return RedirectToAction("Details", new { id = orderId });

            // Nếu sản phẩm đã có trong đơn thì cộng dồn số lượng
            var existing = _context.OrderDetails
                .FirstOrDefault(d => d.OrderId == orderId && d.ProductId == productId);

            if (existing != null)
            {
                existing.Quantity += quantity;
                _context.OrderDetails.Update(existing);
            }
            else
            {
                var detail = new OrderDetail
                {
                    OrderId   = orderId,
                    ProductId = productId,
                    Quantity  = quantity,
                    UnitPrice = product.Price
                };
                _context.OrderDetails.Add(detail);
            }
            _context.SaveChanges();
            TempData["Success"] = "Thêm sản phẩm vào đơn hàng thành công!";
            return RedirectToAction("Details", new { id = orderId });
        }

        // Xoá một dòng chi tiết
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult RemoveDetail(int detailId, int orderId)
        {
            var detail = _context.OrderDetails.Find(detailId);
            if (detail != null)
            {
                _context.OrderDetails.Remove(detail);
                _context.SaveChanges();
                TempData["Success"] = "Đã xoá sản phẩm khỏi đơn hàng!";
            }
            return RedirectToAction("Details", new { id = orderId });
        }
    }
}
