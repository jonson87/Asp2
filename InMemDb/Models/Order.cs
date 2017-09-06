using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDb.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public Cart Cart { get; set; }
        public int CartTotal { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime TimeOfOrder { get; set; }
        [Required]
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }
        [Required]
        [Display(Name = "Lastname")]
        public string Lastname { get; set; }
        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }
    }
}
