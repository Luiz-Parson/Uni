using System;

namespace ConnectorAccess
{
    class AccessControl
    {
        public int Id { get; set; }
        public string Direction { get; set; }
        public int ProductId { get; set; }
        public DateTime AccessedOn { get; set; }
    }
}
