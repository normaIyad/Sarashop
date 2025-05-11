using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sarashop.DTO;
using Sarashop.Models;
using Sarashop.Utility.DataBaseInitulizer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sarashop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<ApplecationUser> userManager;
        private readonly SignInManager<ApplecationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplecationUser> userManager, SignInManager<ApplecationUser> signInManager, IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.roleManager = roleManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Regester([FromBody] RegesterDTO regester)
        {
            var applecationUser = regester.Adapt<ApplecationUser>();
            //var regesterUser = new ApplecationUser
            //{
            //    UserName = regester.UserName,
            //    Email = regester.Email,
            //    firstName = regester.FirstName,
            //    lastName = regester.LastName,
            //    Gender = regester.Gender,
            //    BirthDate = regester.BirthDate
            //};


            var result = await userManager.CreateAsync(applecationUser, regester.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(applecationUser, false);
                await userManager.AddToRoleAsync(applecationUser, StaticData.Customer);
                var token = await userManager.GenerateEmailConfirmationTokenAsync(applecationUser);
                var emailConf = Url.Action(nameof(ConfirmEmail), "Account", new { token, userId = applecationUser.Id }, Request.Scheme);
                await emailSender.SendEmailAsync(applecationUser.Email, "Welcome",
                    $"<h1>Hello {applecationUser.UserName}</h1><a href='{emailConf}'>Click here</a>");

                return Ok("Done");
            }

            return BadRequest(result.Errors);

        }
        [HttpGet("ConfarmEmaile")]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return Ok(new { massege = "welcome " });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return NotFound();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LogINDTO login)
        {

            var userData = await userManager.FindByEmailAsync(login.Email);
            if (userData != null)
            {
                var pass = await userManager.CheckPasswordAsync(userData, login.Password);
                if (pass)
                {
                    List<Claim> claims = new();
                    claims.Add(new(JwtRegisteredClaimNames.Name, userData.UserName));
                    claims.Add(new(JwtRegisteredClaimNames.Sub, userData.Id));
                    var rols = await userManager.GetRolesAsync(userData);
                    if (rols.Count > 0)
                    {
                        foreach (var role in rols)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }
                    }
                    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("pX0BboXxx7FAAhJS8kfdiJJVJp7xkSAO"));
                    var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                    //  SigningCredentials signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                    var jwtToken = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                     );
                    string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                    return Ok(new { token });
                }
            }
            return BadRequest(new { massage = "invaled Emaile or Password" });
        }
        [HttpGet("Logout")]
        public async Task<IActionResult> singout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }
        [Authorize]
        [HttpPost("ChangePassword")]

        public async Task<IActionResult> changePassword([FromBody] ChangePasswordREQ changePassword)
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                var result = await userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
                if (result.Succeeded)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(result);
                }
            }
            return BadRequest(new { masseg = "invaled data " });
        }
    }
}
