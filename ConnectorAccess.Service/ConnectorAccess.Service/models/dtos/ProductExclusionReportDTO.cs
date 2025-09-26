using System;
using System.Collections.Generic;

namespace ConnectorAccess.Service.models.dtos
{
    public class ProductExclusionReportDTO
    {
        public string Description { get; set; }
        public string SKU { get; set; }
        public string EPC { get; set; }
        public string Category { get; set; }
        public int QuantityOfDeletes { get; set; }
        public DateTime ExcludedOn { get; set; }
    }
}
