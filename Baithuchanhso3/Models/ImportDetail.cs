using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiThucHanhSo3.Models
{
    public class ImportDetail
    {
        public int Id { get; set; }

        [Required]
        public int ImportReceiptId { get; set; }
        public ImportReceipt? ImportReceipt { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thiết bị")]
        [Display(Name = "Thiết bị")]
        public int DeviceId { get; set; }
        public Device? Device { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Số lượng phải >= 1")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Required, Range(0, double.MaxValue, ErrorMessage = "Đơn giá nhập phải >= 0")]
        [Display(Name = "Đơn giá nhập")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ImportPrice { get; set; }

        [NotMapped]
        public decimal LineTotal => Quantity * ImportPrice;
    }
}
