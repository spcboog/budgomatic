using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Budgomatic.Core.Domain;

namespace Budgomatic.Web.Models.Transaction
{
    public class IndexModel
    {
        [Required]
        public IEnumerable<Budgomatic.Core.Domain.Transaction> Transactions { get; set; }
    }
}