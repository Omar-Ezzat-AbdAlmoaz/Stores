using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;
using System.Diagnostics.CodeAnalysis;

namespace Mkhz.Models
{

    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Display(Name = "اسم التاجر")]
        public string Client { get; set; }

        [Display(Name = "التاريخ")]
        public DateTime DateTime { get; set; }

        [Display(Name = "الاجمالى")]
        [DefaultValue(0)]
        public decimal Total { get; set; }

        [Display(Name = "المدفوع")]
        [DefaultValue(0)]
        public decimal Paid { get; set; }

        [Display(Name = "المتبقى")]
        [DefaultValue(0)]
        public decimal Residual { get; set; }


        //[Display(Name = "المنتجات"),AllowNull]
        //public List<ProductWithQuantity> ?products { get; set; }


    }
}
