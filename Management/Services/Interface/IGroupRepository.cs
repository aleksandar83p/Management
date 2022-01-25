using Management.Data.DTO;
using Management.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Management.Services.Interface
{
    public interface IGroupRepository
    {
        Task<List<Group>> GetGroupsAsync();
        Task<Group> GetGroupByIdAsync(int groupdId);
        Task<Group> AddGroupAsync(GroupDTO group);
        Task<Group> UpdateGroupByIdAsync(int groupdId, Group group);
        Task DeleteGroupAsync(int groupdId);
    }
}
