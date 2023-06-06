using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bookstore.Data;

namespace Bookstore.Models
{
    [Table("Book")]
    public class BookModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int BookId { get; set; }
      
        [Display(Name = "Título")]
        public string Title { get; set; }
        
        [Display(Name = "Autor")]
        public string Author { get; set; }
        
        [Display(Name = "Editora")]
        public string PublishingCompany { get; set; }
        
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }
        
        [Display(Name = "Quantidade")]
        public int Quantity { get; set; }

        [Display(Name = "Código de Barras")]
        public int BarCode { get; set; }

        [Display(Name = "Valor")]
        public decimal Value { get; set; }

        [Display(Name = "Itens da Venda")]
        public virtual ICollection<SaleItemModel>? SaleItems { get; set; }
    }
}
