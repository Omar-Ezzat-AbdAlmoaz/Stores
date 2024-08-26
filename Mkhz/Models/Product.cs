using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;

namespace Mkhz.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "اسم المنتج")]
        public string NameProduct { get; set; }

        [Required]
        [Display(Name = "اسم الشركة")]
        public string NameCompany { get; set; }

        [Required]
        [Display(Name = "وصف المنتج")]
        public string DescProduct { get; set; }

        [Required]
        [Display(Name = "سعر الشراء")]
        public decimal PurchasingPrice { get; set; }

        [Required]
        [Display(Name = "سعر البيع")]
        public decimal sellingPrice { get; set; }

        [Display(Name = "الكمية المتاحة")]
        [DefaultValue(0)]
        public int ProductQuantity { get; set; }

        [Display(Name = "المبيعات")]
        [DefaultValue(0)]
        public int sales { get; set; }
    }
}
