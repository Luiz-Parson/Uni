using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectorAccess
{
    class ExclusionReportRequestDTO
    {
        public string Description { get; set; }
        public string EPC { get; set; }
        public string SKU { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
