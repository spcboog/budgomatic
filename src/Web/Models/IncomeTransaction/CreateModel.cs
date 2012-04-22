using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Budgomatic.Web.Models.IncomeTransaction
{
    public class CreateModel
    {
        [Required]
        public IEnumerable<Budgomatic.Core.Domain.Account> IncomeAccounts { get; set; }

        [Required]
        public IEnumerable<Budgomatic.Core.Domain.Account> AssetAccounts { get; set; }

        [Required]
        [Display(Name="Income source")]
        public Guid SelectedIncomeAccountId { get; set; }

        [Required]
        [Display(Name = "Account to transfer income to")]
        public Guid SelectedAssetAccountId { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }

        [Required]
        [Range(0.01, 1000000000)]
        public decimal Amount { get; set; }

        public string Comments { get; set; }
    }
}