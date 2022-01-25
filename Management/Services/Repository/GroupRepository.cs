using Management.Data;
using Management.Data.DTO;
using Management.Data.Models;
using Management.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Management.Services.Repository
{
    public class GroupRepository : IDisposable, IGroupRepository
    {
        private AppDbContext _AppDbContext;     
        public GroupRepository(AppDbContext appDbContext)
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

        public async Task<List<Group>> GetGroupsAsync()
        {
            return await _AppDbContext.Groups.ToListAsync();
        }

        public async Task<Group> GetGroupByIdAsync(int groupdId)
        {
            return await _AppDbContext.Groups.FirstOrDefaultAsync(x => x.Id == groupdId);
        }

        public async Task<Group> AddGroupAsync(GroupDTO groupDTO)
        {
            var newGroup = new Group();
            newGroup.Name = groupDTO.Name;

            await _AppDbContext.Groups.AddAsync(newGroup);
            await _AppDbContext.SaveChangesAsync();

            return newGroup;
        }

        public async Task<Group> UpdateGroupByIdAsync(int groupdId, Group group)
        {
            var updateGroup = await _AppDbContext.Groups.FirstOrDefaultAsync(x => x.Id == groupdId);

            if (updateGroup != null)
            {
                updateGroup.Name = group.Name;                

                await _AppDbContext.SaveChangesAsync();

                return updateGroup;
            }

            return null;
        }

        public async Task DeleteGroupAsync(int groupdId)
        {
            var deleteGroup = await _AppDbContext.Groups.FirstOrDefaultAsync(x => x.Id == groupdId);

            if (deleteGroup != null)
            {
                _AppDbContext.Groups.Remove(deleteGroup);
                await _AppDbContext.SaveChangesAsync();
            }          
        }
    }
}
