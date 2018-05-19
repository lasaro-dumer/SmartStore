using System.ComponentModel.DataAnnotations;

namespace SmartStore.Data.Models
{
    public class UserDetails
    {
        public string Email { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Email confirmed")]
        public bool EmailConfirmed { get; set; }
    }
}
