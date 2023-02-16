using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CursoMOD119.Models
{
    public class StockMovement
    {
        public int ID { get; set; }

        // True => Entrada de stock, False => Saída de stock
        public bool Type { get; set; }

        // Movement Quantity
        public int Amount { get; set; }

        // Date of Movement
        public DateTime MovementDate { get; set; }

        // Foreign Key
        [Display(Name = "Item")]
        public int ItemID { get; set; }

        public Item? Item { get; set; }
    }
}
