using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    [Table("SaleItem")]
    public class SaleItemModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")] 
        public int SaleItemId { get; set; }

        [Display(Name = "Valor")]
        [DataType(DataType.Currency)]
        public decimal Value { get; set; }

        [Display(Name = "Quantidade")]
        public int Quantity { get; set; }

        [Display(Name = "Subtotal")]
        [DataType(DataType.Currency)]
        public decimal Subtotal { get; set; }

        [Display(Name = "Livro ID")]
        public int BookId { get; set; }

        [Display(Name = "Livro")]
        public virtual BookModel? Book { get; set; }

        [Display(Name = "Venda ID")]
        public int SaleId { get; set; }

        [Display(Name = "Venda")]
        public virtual SaleModel? Sale { get; set; }

    }
}
