using Sarashop.DataBase;
using Sarashop.Models;
using Sarashop.service.IServices;

namespace Sarashop.service
{
    public class PasswordResetCodeService : Service<PasswordResetCode>, IPasswordResetCodeService
    {
        private readonly DatabaseConfigration databaseConfigration;

        public PasswordResetCodeService(DatabaseConfigration databaseConfigration) : base(databaseConfigration)
        {
            this.databaseConfigration = databaseConfigration;
        }
    }
}
