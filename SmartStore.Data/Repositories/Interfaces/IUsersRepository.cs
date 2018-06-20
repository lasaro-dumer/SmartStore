using SmartStore.Data.Entities;

namespace SmartStore.Data.Repositories.Interfaces
{
    public interface IUsersRepository : IBaseRepository
    {
        UserEntity GetUserByUsername(string username);
        UserEntity GetUserById(string userId);
        UserEntity GetUserByConfirmationToken(string token);
    }
}
