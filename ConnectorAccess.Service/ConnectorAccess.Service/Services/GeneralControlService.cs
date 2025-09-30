using ConnectorAccess.Service.data;
using ConnectorAccess.Service.models;
using System;
using System.Linq;

namespace ConnectorAccess.Service.Services
{
    public class GeneralControlService
    {

        private readonly ConnectorDbContext context;

        public GeneralControlService(ConnectorDbContext context)
        {
            this.context = context;
        }

        public GeneralControl AddGeneralControl(int productId, DateTime accessedOn)
        {
            var generalControl = new GeneralControl
            {
                ProductId = productId,
                AccessedOn = accessedOn
            };

            context.GeneralControl.Add(generalControl);
            context.SaveChanges();

            return generalControl;
        }
    }
}
