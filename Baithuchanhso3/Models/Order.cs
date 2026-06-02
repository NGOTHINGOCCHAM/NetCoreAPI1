using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiThucHanhSo3.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "Ngày đặt")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [StringLength(200)]
        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

        // Khoá ngoại: 1 đơn hàng thuộc 1 khách hàng
        [Required(ErrorMessage = "Vui lòng chọn khách hàng")]
        [Display(Name = "Khách hàng")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        // 1 đơn hàng có nhiều chi tiết
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        // Tổng tiền tính toán
        [NotMapped]
        public decimal TotalAmount => OrderDetails.Sum(d => d.LineTotal);
    }
}
