﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Budgomatic.Web.Models.TransferTransaction
{
    public class EditModel : CreateModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}