using System.ComponentModel.DataAnnotations;

namespace Bookstore.Commun
{
    public enum SaleStatus
    {
        [Display(Name = "Aberta")]
        Open = 0,

        [Display(Name = "Fechada")]
        Closed,

        [Display(Name = "Cancelada")]
        Canceled
    }
}
