using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Budgomatic.Web.Models.TransferTransaction
{
    public class CreateModel
    {
        [Required]
        public IEnumerable<Budgomatic.Core.Domain.Account> AssetAndLiabilityAccounts { get; set; }

        [Required]
        [Display(Name="From")]
        public Guid SelectedFromAccountId { get; set; }

        [Required]
        [Display(Name = "To")]
        public Guid SelectedToAccountId { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }

        [Required]
        [Range(0.01, 1000000000)]
        public decimal Amount { get; set; }

        public string Comments { get; set; }
    }
}