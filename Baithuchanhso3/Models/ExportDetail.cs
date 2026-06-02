using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiThucHanhSo3.Models
{
    public class ExportDetail
    {
        public int Id { get; set; }

        [Required]
        public int ExportReceiptId { get; set; }
        public ExportReceipt? ExportReceipt { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thiết bị")]
        [Display(Name = "Thiết bị")]
        public int DeviceId { get; set; }
        public Device? Device { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Số lượng phải >= 1")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Required, Range(0, double.MaxValue, ErrorMessage = "Đơn giá xuất phải >= 0")]
        [Display(Name = "Đơn giá xuất")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ExportPrice { get; set; }

        [NotMapped]
        public decimal LineTotal => Quantity * ExportPrice;
    }
}
