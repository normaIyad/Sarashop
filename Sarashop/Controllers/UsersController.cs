using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sarashop.DTO;
using Sarashop.Models;
using Sarashop.Utility.DataBaseInitulizer;

namespace Sarashop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = StaticData.SuperAdmin)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplecationUser> userManager;

        public UsersController(UserManager<ApplecationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var users = userManager.Users.ToList();
            return Ok(users.Adapt<IEnumerable<UserDTO>>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await userManager.FindByIdAsync(id);
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
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var currentRoles = await userManager.GetRolesAsync(user);
            foreach (var role in currentRoles)
            {
                var removeResult = await userManager.RemoveFromRoleAsync(user, role);
                if (!removeResult.Succeeded)
                {
                    return BadRequest($"Failed to remove role {role}");
                }
            }

            var addResult = await userManager.AddToRoleAsync(user, roleName);
            if (!addResult.Succeeded)
            {
                return BadRequest($"Failed to add role {roleName}");
            }

            return Ok("Role updated successfully");
        }
    }
}
