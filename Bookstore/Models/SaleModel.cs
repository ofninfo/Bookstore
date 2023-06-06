using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

using Bookstore.Commun;

namespace Bookstore.Models
{
    [Table("Sale")]
    public class SaleModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int SaleId { get; set; }

        [Display(Name = "Subtotal")]
        [DataType(DataType.Currency)] 
        public decimal Subtotal { get; set; }

        [Display(Name = "Desconto")]
        [DataType(DataType.Currency)]
        public decimal Discount { get; set; }

        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        [Display(Name = "Data")]
        public DateTime Date { get; set; }

        [Display(Name = "Status")]
        public SaleStatus Status { get; set; }

        [Display(Name = "Itens da Venda")]
        public virtual ICollection<SaleItemModel> SaleItems { get; set; } = new List<SaleItemModel>();

        [Display(Name = "Cliente ID")]
        public int? ClientId { get; set; }

        [Display(Name = "Cliente")]
        public virtual ClientModel? Client { get; set; }
    }
}
