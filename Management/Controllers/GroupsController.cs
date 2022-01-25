using Management.Data.DTO;
using Management.Data.Models;
using Management.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupRepository _GroupRepository;


        public GroupsController(IGroupRepository groupRepository)
        {
            _GroupRepository = groupRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroupsAsync()
        {
            try
            {
                var group = await _GroupRepository.GetGroupsAsync();

                return Ok(group);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetGroupByIdAsync(int id)
        {
            try
            {
                var group = await _GroupRepository.GetGroupByIdAsync(id);

                if (group == null)
                {
                    return NotFound($"Group with ID = {id} not found.");
                }

                return Ok(group);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateGroupAsync([FromBody] GroupDTO groupDTO)
        {
            try
            {
                if (groupDTO == null)
                {
                    return BadRequest();
                }

                var newGroup = await _GroupRepository.AddGroupAsync(groupDTO);
                return Created(nameof(CreateGroupAsync), newGroup);               
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new user record");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateGroupAsync(int id, Group group)
        {
            try
            {
                if (id != group.Id)
                {
                    return BadRequest("Group ID mismatch");
                }

                var groupToUpdate = await _GroupRepository.GetGroupByIdAsync(id);

                if (groupToUpdate == null)
                {
                    return NotFound($"Group with ID = {id} not found.");
                }

                return Ok(await _GroupRepository.UpdateGroupByIdAsync(id, group));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating user record");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteGroupAsync(int id)
        {
            try
            {
                var deleteGroup = await _GroupRepository.GetGroupByIdAsync(id);

                if (deleteGroup == null)
                {
                    return BadRequest($"Group with ID = {id} not found.");
                }

                await _GroupRepository.DeleteGroupAsync(id);
                return Ok($"Group with ID = {id} deleted.");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting user record");
            }
        }
    }
}
