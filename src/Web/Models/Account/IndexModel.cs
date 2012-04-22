using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Budgomatic.Core.Domain;

namespace Budgomatic.Web.Models.Account
{
    public class IndexModel
    {
        [Required]
        public Budgomatic.Core.Domain.Account SelectedAccount { get; set; }

        [Required]
        public IEnumerable<Budgomatic.Core.Domain.Account> Accounts { get; set; }
    }
}