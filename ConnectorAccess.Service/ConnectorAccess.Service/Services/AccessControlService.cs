using ConnectorAccess.Service.data;
using ConnectorAccess.Service.models;
using System;
using System.Linq;

namespace ConnectorAccess.Service.Services
{
    public class AccessControlService
    {

        private readonly ConnectorDbContext context;

        public AccessControlService(ConnectorDbContext context)
        {
            this.context = context;
        }

        public AccessControl GetLastAccessControlByProductId(int productId)
        {
            var accessControl = context.AccessControl
                .Where(ac => ac.ProductId == productId)
                .OrderByDescending(ac => ac.AccessedOn)
                .FirstOrDefault();

            return accessControl;
        }

        public AccessControl AddAccessControl(string direction, int productId, DateTime accessedOn, string status)
        {
            var accessControl = new AccessControl
            {
                Direction = direction,
                ProductId = productId,
                AccessedOn = accessedOn,
                Status = status
            };

            context.AccessControl.Add(accessControl);
            context.SaveChanges();

            return accessControl;
        }
    }
}
