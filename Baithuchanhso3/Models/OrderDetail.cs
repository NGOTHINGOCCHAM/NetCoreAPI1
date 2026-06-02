using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiThucHanhSo3.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        // Khoá ngoại đến Order
        [Required]
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        // Khoá ngoại đến Product
        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        [Display(Name = "Sản phẩm")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải >= 1")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Đơn giá không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải >= 0")]
        [Display(Name = "Đơn giá")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Thành tiền = Số lượng * Đơn giá
        [NotMapped]
        public decimal LineTotal => Quantity * UnitPrice;
    }
}
