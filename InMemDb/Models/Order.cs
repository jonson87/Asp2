using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDb.Models
{
    public class Order
    {
        public Cart Cart { get; set; }
        public int CartTotal { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime TimeOfOrder { get; set; }
    }
}
