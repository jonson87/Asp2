using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDb.Models
{
    public class CreditCard
    {
        [Required]
        [DisplayName("Name On Card")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Card Number")]
        [DataType(DataType.CreditCard)]
        public int CardNumber { get; set; }
        [Required]
        [DisplayName("Expiration Date")]
        public int ExpirationDate { get; set; }
        [Required]
        [DisplayName("Card CVV")]
        public int Cvv { get; set; }

    }
}
