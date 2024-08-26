using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mkhz.Models
{
    public class Expens
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required,Display(Name = "مصروفات")]
        public string NameExpens { get; set; }

        [Display(Name = "التاريخ")]
        public DateTime DateTimeExpens { get; set; }

        [Required,Display(Name = "المبلغ")]
        public decimal Total { get; set; }

    }
}
