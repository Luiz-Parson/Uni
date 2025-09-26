using ConnectorAccess.Service.data;
using ConnectorAccess.Service.models;
using System;
using System.Linq;

namespace ConnectorAccess.Service.Services
{
    public class SystemUserService
    {
        private readonly ConnectorDbContext context;

        public SystemUserService(ConnectorDbContext context)
        {
            this.context = context;
        }

        public void addSystemUser(string username, string password, bool isAdmin, string createdBy)
        {
            var systemUser = new SystemUser
            {
                Username = username,
                Password = password,
                IsAdmin = isAdmin,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                UpdatedBy = createdBy,
                UpdatedOn = DateTime.Now
            };

            context.SystemUser.Add(systemUser);
            context.SaveChanges();
        }

        public SystemUser Login(string username, string password)
        {
            string encryptedPassword = EncryptionHelper.Encrypt(password);
            return context.SystemUser
                .FirstOrDefault(u => u.Username == username && u.Password == encryptedPassword);
        }
    }
}
