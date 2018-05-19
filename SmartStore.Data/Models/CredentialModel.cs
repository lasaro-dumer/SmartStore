using System.ComponentModel.DataAnnotations;

namespace SmartStore.Data.Models
{
    public class CredentialModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
