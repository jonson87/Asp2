using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDb.Models.CheckOutViewModel
{
    public class CheckoutViewModel
    {
        public Cart Cart { get; set; }
        public ApplicationUser User { get; set; }
        public CreditCard CreditCard { get; set; }
    }
}
