using Microsoft.AspNetCore.Identity;
using Sarashop.DataBase;
using Sarashop.Models;
using Sarashop.service.IServices;

namespace Sarashop.service
{
    public class UserService : Service<ApplecationUser>, IUserService
    {
        private readonly UserManager<ApplecationUser> userManager;
        private readonly DatabaseConfigration databaseConfigration;
        public UserService(DatabaseConfigration databaseConfigration, UserManager<ApplecationUser> userManager) : base(databaseConfigration)
        {
            this.databaseConfigration = databaseConfigration;
            this.userManager = userManager;
        }

        public async Task<bool?> BlockUnBlock(string userID)
        {
            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return null;
            }
            var isBlock = user.LockoutEnabled && user.LockoutEnd > DateTime.Now;
            if (isBlock)
            {
                user.LockoutEnabled = false;
                user.LockoutEnd = null;
            }
            else
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTime.Now.AddMinutes(20);
            }
            var result = await userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ChangeRole(string userID, string roleName)
        {
            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return false;
            }

            var currentRoles = await userManager.GetRolesAsync(user);
            foreach (var role in currentRoles)
            {
                var removeResult = await userManager.RemoveFromRoleAsync(user, role);
                if (!removeResult.Succeeded)
                {
                    return false;
                }
            }

            var addResult = await userManager.AddToRoleAsync(user, roleName);
            if (!addResult.Succeeded)
            {
                return false;
            }

            return true;
        }
    }
}
