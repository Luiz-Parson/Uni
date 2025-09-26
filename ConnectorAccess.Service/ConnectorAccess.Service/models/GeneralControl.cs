using System;

namespace ConnectorAccess.Service.models
{
    public class GeneralControl
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public DateTime AccessedOn { get; set; }
    }
}
