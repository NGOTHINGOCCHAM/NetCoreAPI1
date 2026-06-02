using System.ComponentModel.DataAnnotations;

namespace BaiThucHanhSo3.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên khách hàng không được để trống")]
        [StringLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        [Display(Name = "Tên khách hàng")]
        public string FullName { get; set; } = "";

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [StringLength(200)]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        // 1 khách hàng có nhiều đơn hàng
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
