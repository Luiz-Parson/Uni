using System;
using System.Collections.Generic;

namespace ConnectorAccess.Service.models.dtos
{
    public class ExclusionReportRequestDTO
    {
        public string Description { get; set; }
        public string EPC { get; set; }
        public string SKU { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
