using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InMemDb.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }
        [Display(Name = "Lastname")]
        public string Lastname { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Street")]
        public string Street { get; set; }
    }
}
