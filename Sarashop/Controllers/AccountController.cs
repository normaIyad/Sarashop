using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sarashop.DTO;
using Sarashop.Models;
using Sarashop.service;
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
        private readonly IPasswordResetCodeService passwordResetCode;

        public AccountController(UserManager<ApplecationUser> userManager, SignInManager<ApplecationUser> signInManager, IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager, IPasswordResetCodeService passwordResetCode
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.roleManager = roleManager;
            this.passwordResetCode = passwordResetCode;
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
            var user = await userManager.FindByEmailAsync(login.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, login.Password))
                return BadRequest(new { message = "Invalid email or password" });

            if (!await userManager.IsEmailConfirmedAsync(user))
                return BadRequest(new { message = "Email not confirmed" });
            //IsLockedOut
            if (await userManager.IsLockedOutAsync(user))
            {
                return BadRequest("Your account is temporarily locked. Try again later.");
            }

            var claims = new List<Claim>
            {
                 new(JwtRegisteredClaimNames.Name, user.UserName),
               new(JwtRegisteredClaimNames.Email, user.Email)
            };

            var roles = await userManager.GetRolesAsync(user);
            var id = await userManager.GetUserIdAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, id));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("pX0BboXxx7FAAhJS8kfdiJJVJp7xkSAO"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               issuer: "yourdomain.com",
               audience: "yourdomain.com",
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(30),
               signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
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

        [HttpPost("forgetPassword")]
        public async Task<IActionResult> forgetPassword([FromBody] ForgetPassword Request)
        {
            var user = await userManager.FindByEmailAsync(Request.Email);
            if (user is not null)
            {
                var code = new Random().Next(1000, 9999).ToString();
                await passwordResetCode.AddAsync(new()
                {
                    applecationUserId = user.Id,
                    Code = code,
                    ExprationCode = DateTime.Now.AddMinutes(30),
                });
                await emailSender.SendEmailAsync(user.Email, "forget password",
                   $"<h1>Dar : {user.UserName}</h1>" +
                   $"<h3>you wont to change your password  this is a code for change it </h3>" +
                   $"<h2>{code}</h2>");
                return Ok(new { massege = "code send to your email" });
            }
            else
            {
                return BadRequest(new { massege = "not found Emale" });
            }

        }
        [HttpPost("ChangePasswordPyCode")]
        public async Task<IActionResult> CheckCode(SendCodeReq request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is not null)
            {
                var res = (await passwordResetCode.GetAsync(e => e.applecationUserId == user.Id))
                      .OrderByDescending(e => e.ExprationCode).FirstOrDefault();
                if (res is not null && res.Code == request.Code && res.ExprationCode >= DateTime.Now)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await userManager.ResetPasswordAsync(user, token, request.Password);
                    if (result.Succeeded)
                    {
                        await emailSender.SendEmailAsync(user.Email, "change password", $"<h2>Dear {user.FirstName}<h2>" +
                            $"<h4>your password have been changed successfully</h4>"
                            );
                        await passwordResetCode.RemoveAsync(res.Id);
                        return Ok(new { massege = "Pasword has been change " });
                    }
                    else
                    {
                        return BadRequest(result.Errors);
                    }
                }
                else
                {
                    return BadRequest(new { massege = "invaled code" });
                }
            }
            return BadRequest(new { massege = "not valled emisle" });

        }

    }

}
