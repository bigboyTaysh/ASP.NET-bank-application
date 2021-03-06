﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Basket Basket { get; set; }
        public virtual List<Order> Orders { get; set; }
    }
}
