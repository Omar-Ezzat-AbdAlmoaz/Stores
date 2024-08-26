using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Mkhz.Models
{
    public class ProductWithQuantity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int InvoiceId { get; set; }
        public bool confirmed { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}
