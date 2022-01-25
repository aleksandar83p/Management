
using Management.Data;
using Management.Data.DTO;
using Management.Data.Models;
using Management.Helper;
using Management.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Management.Services.Repository
{
    public class UserRepository : IDisposable, IUserRepository
    {
        private AppDbContext _AppDbContext;       

        public UserRepository(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;          
        }

        
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_AppDbContext != null)
                {
                    _AppDbContext.Dispose();
                    _AppDbContext = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _AppDbContext.Users.Include(x => x.Group).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _AppDbContext.Users.Include(x => x.Group).FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<User> AddUserAsync(UserDTO userDTO)
        {
            userDTO.Password = PasswordHash.ComputeSha256Hash(userDTO.Password);

            var newUser = new User();
            newUser.Name = userDTO.Name;
            newUser.Email = userDTO.Email;
            newUser.Password = userDTO.Password;
            newUser.GroupId = userDTO.GroupId;

            await _AppDbContext.Users.AddAsync(newUser);
            await _AppDbContext.SaveChangesAsync();

            return newUser;
        }

        public async Task<User> UpdateUserByIdAsync(int userId, UserDTOUpdate userDTOUpdate)
        {
            var updateUser = await _AppDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            
            if(updateUser != null)
            {
                updateUser.Name = userDTOUpdate.Name;
                updateUser.Password = PasswordHash.ComputeSha256Hash(userDTOUpdate.Password);
                updateUser.GroupId = userDTOUpdate.GroupId;

                await _AppDbContext.SaveChangesAsync();

                return updateUser;
            }

            return null;
        }

        public async Task DeleteUserAsync(int userId)
        {
            var deleteUser = await _AppDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if(deleteUser != null)
            {
                _AppDbContext.Users.Remove(deleteUser);
                await _AppDbContext.SaveChangesAsync();
            }
        }
    }
}
