using System;

namespace ConnectorAccess.Service.models.dtos
{
    public class AccessControlDTO
    {
        public string Direction { get; set; }
        public int ProductId { get; set; }
        public DateTime AccessedOn { get; set; }
        public string Status { get; set; }
    }
}
