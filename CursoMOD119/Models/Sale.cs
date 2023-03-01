using CursoMOD119.Models.SalesViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoMOD119.Models
{
    public class Sale
    {
        public int ID { get; set; }

        [Display(Name = "Sale Date")]
        public DateTime SaleDate { get; set; }

        [Display(Name = "Amount")]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public int ClientID { get; set; }

        
        public Client Client { get; set; }
        
        [Display(Name = "Items")]
        public ICollection<Item> Items { get; set; }



        public static implicit operator SaleViewModel(Sale sale)
        {
            if (sale == null)
            {
                return null;
            }

            return new SaleViewModel
            {
                ID = sale.ID,
                SaleDate = sale.SaleDate,
                Amount = sale.Amount,
                ClientID = sale.ClientID
            };
        }


        public static implicit operator Sale(SaleViewModel saleViewModel)
        {
            if (saleViewModel == null)
            {
                return null;
            }

            return new Sale
            {
                ID = saleViewModel.ID,
                SaleDate = saleViewModel.SaleDate,
                Amount = saleViewModel.Amount,
                ClientID = saleViewModel.ClientID
            };
        }

        



    }
}
