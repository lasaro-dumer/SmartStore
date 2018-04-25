using System.ComponentModel.DataAnnotations;

namespace SmartStore.Web.Portal.Models
{
    public class NewUserModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(256)]
        [Display(Name = "Username")]
        [RegularExpression(@"^[a-z-A-Z]+[a-zA-Z0-9_.]*", ErrorMessage = "Your username must start with a letter and contain only: alphanumeric characters; dots and underlines")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The passwords must match")]
        [Display(Name = "Confirm the password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "I accept the terms and conditions")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms and conditions")]
        public bool AcceptedTerms { get; set; }
    }
}
