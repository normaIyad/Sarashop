using Mapster;
using Microsoft.AspNetCore.Mvc;
using Sarashop.DTO;
using Sarashop.service;

namespace Sarashop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = StaticData.SuperAdmin)]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var users = userService.GetAsync();
            return Ok(users.Adapt<IEnumerable<UserDTO>>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {//_brandService.GetOne(b => b.Id == id)
            var user = await userService.GetOne(u => u.Id == id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var userDto = user.Adapt<UserDTO>();
            return Ok(userDto);
        }


        [HttpPost("change-role")]
        public async Task<IActionResult> ChangeRole(string id, string roleName)
        {
            var result = await userService.ChangeRole(id, roleName);
            if (result)
            {
                return Ok("Role updated successfully");
            }
            return BadRequest("Error in user data");
        }
        [HttpPatch("blockUnblock/{userId}")]
        public async Task<IActionResult> BlockUnBlock(string userId)
        {
            var result = await userService.BlockUnBlock(userId);
            if (result.HasValue)
            {
                if (result.Value)
                {
                    return Ok("Changed block state successfully.");
                }
                return BadRequest("Failed operation.");
            }

            return NotFound("User not found.");
        }

    }
}
