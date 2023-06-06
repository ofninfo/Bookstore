using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    [Table("Client")]
    public class ClientModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ClientId { get; set; }

        [Display(Name = "Nome Completo")]
        public string Name { get; set; }

        [Display(Name = "CPF")]
        public string CPF { get; set; }

        [Display(Name = "Endereço")]
        public string Address { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Data de Nascimento")]
        public DateTime DateBirth { get; set; }

        [Display(Name = "Vendas")]
        public virtual ICollection<SaleModel>? Sales { get; set; }

    }
}
