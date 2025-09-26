using System;

namespace ConnectorAccess.Service.models
{
    public class AccessControl
    {
        public int Id { get; set; }
        public string Direction { get; set; }
        public int ProductId { get; set; }
        public DateTime AccessedOn { get; set; }
        public string Status { get; set; }
    }
}
