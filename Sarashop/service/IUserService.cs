using Sarashop.Models;
using Sarashop.service.IServices;

namespace Sarashop.service
{
    /// <summary>
    ///  IService<Category>
    /// </summary>
    public interface IUserService : IService<ApplecationUser>
    {
        Task<bool> ChangeRole(string userID, string roleName);
        Task<bool?> BlockUnBlock(string userID);
    }
}
