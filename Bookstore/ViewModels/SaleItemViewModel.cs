using Bookstore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Bookstore.ViewModels
{
    public class SaleItemViewModel
    {
        [Display(Name = "ID")]
        public int SaleItemId { get; set; }

        [Display(Name = "Valor")]
        public decimal Value { get; set; }

        [Display(Name = "Quantidade")]
        public int Quantity { get; set; }

        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        [Display(Name = "Livro ID")]
        public int BookId { get; set; }

        [NotMapped]
        public List<SelectListItem> Books { get; set; }

        [Display(Name = "Venda ID")]
        public int SaleId { get; set; }

        [NotMapped]
        public List<SelectListItem> Sales { get; set; }

    }
}
