using System;

namespace ConnectorAccess.Service.models.dtos
{
    public class ReportDTO
    {
        public string Description { get; set; } = "";
        public string Epc { get; set; } = "";
        public string Sku { get; set; } = "";
        public DateTime InitialDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
