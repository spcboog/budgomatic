using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Budgomatic.Core.Domain;

namespace Budgomatic.Web.Models.Account
{
    public class CreateModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public AccountType AccountType { get; set; }
    }
}