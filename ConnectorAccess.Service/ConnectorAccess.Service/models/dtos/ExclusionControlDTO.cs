using System;

namespace ConnectorAccess.Service.models.dtos
{
    public class ExclusionControlDTO
    {
        public int ProductId { get; set; }
        public string Category { get; set; }
        public DateTime ExcludedOn { get; set; }
    }
}
