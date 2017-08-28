﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDb.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int PersonId { get; set; }
        public List<CartItem> CartItem { get; set; }
    }
}
