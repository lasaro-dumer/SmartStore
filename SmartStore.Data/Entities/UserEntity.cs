using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SmartStore.Data.Entities
{
    public class UserEntity : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailConfirmationToken { get; set; }
        public DateTime EmailConfirmationExpiration { get; set; }
        [MaxLength(50)]
        public string CreditCardNumber { get; set; }
        [MaxLength(50)]
        public string CreditCardCompany { get; set; }
    }
}
