using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Budgomatic.Core.Domain;

namespace Budgomatic.Web.Models.Account
{
    public class EditModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public AccountType AccountType { get; set; }
    }
}