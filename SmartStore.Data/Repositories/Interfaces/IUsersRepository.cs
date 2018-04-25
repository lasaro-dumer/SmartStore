using SmartStore.Data.Entities;

namespace SmartStore.Data.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        UserEntity GetUserByUsername(string username);
    }
}
