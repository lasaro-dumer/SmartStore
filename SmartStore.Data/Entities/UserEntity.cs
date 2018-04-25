using Microsoft.AspNetCore.Identity;

namespace SmartStore.Data.Entities
{
    public class UserEntity : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
