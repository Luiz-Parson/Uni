using System;

namespace ConnectorAccess.Service.models
{
    public class ExclusionControl
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Category { get; set; }
        public DateTime ExcludedOn { get; set; }
    }
}
