using System.Linq;
using SmartStore.Data.Entities;
using SmartStore.Data.Repositories.Interfaces;

namespace SmartStore.Data.Repositories
{
    public class UsersRepository : BaseRepository, IUsersRepository
    {
        public UsersRepository(SmartStoreDbContext context) : base(context)
        {
        }

        public UserEntity GetUserByUsername(string username)
        {
            var user = _context.Users.Where(u => u.UserName == username).FirstOrDefault();

            return user;
        }
    }
}
