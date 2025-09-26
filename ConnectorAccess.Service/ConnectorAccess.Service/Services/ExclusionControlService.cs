using ConnectorAccess.Service.data;
using ConnectorAccess.Service.models;
using ConnectorAccess.Service.models.dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConnectorAccess.Service.Services
{
    public class ExclusionControlService
    {

        private readonly ConnectorDbContext context;

        public ExclusionControlService(ConnectorDbContext context)
        {
            this.context = context;
        }

        public ExclusionControl AddExclusionControl(int productId, string category, DateTime ExcludedOn)
        {
            var exclusionControl = new ExclusionControl
            {
                ProductId = productId,
                Category = category,
                ExcludedOn = ExcludedOn
            };

            context.ExclusionControl.Add(exclusionControl);
            context.SaveChanges();

            return exclusionControl;
        }

        public List<ExclusionControl> AddMultipleExclusions(List<ExclusionControlDTO> exclusions)
        {
            var exclusionEntities = exclusions.Select(dto => new ExclusionControl
            {
                ProductId = dto.ProductId,
                Category = dto.Category,
                ExcludedOn = dto.ExcludedOn
            }).ToList();

            context.ExclusionControl.AddRange(exclusionEntities);
            context.SaveChanges();

            return exclusionEntities;
        }
    }
}
