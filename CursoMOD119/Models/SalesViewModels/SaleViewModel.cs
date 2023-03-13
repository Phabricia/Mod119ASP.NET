﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CursoMOD119.Models.ItemsViewModels;

namespace CursoMOD119.Models.SalesViewModels
{
    public class SaleViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Sale Date")]
        [DataType(DataType.DateTime)]
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Display(Name = "Amount")]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Display(Name = "Client")]
        public int ClientID { get; set; }

        [Display(Name = "Items")]
        public int[]? ItemIDs { get; set; } = Array.Empty<int>();

        public List<SelectableItemViewModel> SelectableItems { get; set; }
    }
}
