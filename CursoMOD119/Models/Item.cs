using CursoMOD119.Models.ItemsViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoMOD119.Models
{
    public class Item
    {

        public int ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; } = "";

        [Display(Name = "Price")]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Range(0.00, 999999999.99, ErrorMessage = "RangeErrorMessage")]
        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Discontinued")]
        public bool Discontinued { get; set; }

        [Display(Name = "Sales")]
        public ICollection<Sale>? Sales { get; set; }


        public static implicit operator SelectableItemViewModel(Item item)
        {
            if (item == null)
            {
                return null;
            }

            return new SelectableItemViewModel
            {
                ID = item.ID,
                Name = item.Name,
                Price = item.Price,
                Selected = false
            };
        }

        public static implicit operator Item(SelectableItemViewModel viewModel)
        {
            if (viewModel == null)
            {
                return null;
            }

            return new Item
            {
                ID = viewModel.ID,
                Name = viewModel.Name,
                Price = viewModel.Price
            };
        }
    }
}
