using Management.Data.DTO;
using Management.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Management.Services.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<User> AddUserAsync(UserDTO userDTO);
        Task<User> UpdateUserByIdAsync(int userId, UserDTOUpdate user);
        Task DeleteUserAsync(int userId);
    }
}
