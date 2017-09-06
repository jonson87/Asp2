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
        [StringLength(16, MinimumLength =16, ErrorMessage ="Must be 16 digits")]
        public string CardNumber { get; set; }

        [Required]
        [DisplayName("Expiration Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{y}")]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [DisplayName("Card CVV")]
        [StringLength(3, MinimumLength =3, ErrorMessage ="Must be 3 digits")]
        public string Cvv { get; set; }
    }
}
