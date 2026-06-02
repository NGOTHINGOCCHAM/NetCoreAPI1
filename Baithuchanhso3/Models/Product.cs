using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiThucHanhSo3.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(150, ErrorMessage = "Tối đa 150 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; } = "";

        [Required(ErrorMessage = "Đơn giá không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải >= 0")]
        [Display(Name = "Đơn giá")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [StringLength(300)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
