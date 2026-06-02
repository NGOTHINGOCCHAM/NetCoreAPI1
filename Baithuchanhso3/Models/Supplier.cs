using System.ComponentModel.DataAnnotations;

namespace BaiThucHanhSo3.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống")]
        [StringLength(150)]
        [Display(Name = "Tên nhà cung cấp")]
        public string SupplierName { get; set; } = "";

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Điện thoại")]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [StringLength(200)]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        public ICollection<ImportReceipt> ImportReceipts { get; set; } = new List<ImportReceipt>();
    }
}
