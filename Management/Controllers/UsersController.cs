using Management.Data.DTO;
using Management.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _UserRepository;
 

        public UsersController(IUserRepository userRepository)
        {
            _UserRepository = userRepository;          
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                var users = await _UserRepository.GetUsersAsync();

                return Ok(users);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
            
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _UserRepository.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound($"User with ID = {id} not found.");
                }

                return Ok(user);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
          
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserAsync([FromBody]UserDTO userDTO)
        {
            try
            {
                if(userDTO == null)
                {
                    return BadRequest();
                }

                var newUser = await _UserRepository.AddUserAsync(userDTO);
                return Created(nameof(CreateUserAsync), newUser);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new user record");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUserAsync(int id, UserDTOUpdate userDTOUpdate)
        {
            try
            {
                if(id != userDTOUpdate.Id)
                {
                    return BadRequest("User ID mismatch");
                }

                var userToUpdate = await _UserRepository.GetUserByIdAsync(id);

                if(userToUpdate == null)
                {
                    return NotFound($"User with ID = {id} not found.");
                }

                return Ok(await _UserRepository.UpdateUserByIdAsync(id, userDTOUpdate));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating user record");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            try
            {
                var deleteUser = await _UserRepository.GetUserByIdAsync(id);

                if(deleteUser == null)
                {
                    return BadRequest($"User with ID = {id} not found.");
                }

                await _UserRepository.DeleteUserAsync(id);
                return Ok($"User with ID = {id} deleted.");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting job candidate record");
            }
        }
    }
}
