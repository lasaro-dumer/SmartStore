using System.ComponentModel.DataAnnotations;

namespace SmartStore.Web.Portal.Models
{
    public class BillingInformationModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Credit Card Number")]
        [RegularExpression("[0-9]+")]
        public string CreditCardNumber { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Credit Card Company")]
        public string CreditCardCompany { get; set; }
    }
}
