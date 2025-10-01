using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectorAccess
{
    class GeneralReportRequestDTO
    {
        public string Description { get; set; }
        public string Epc { get; set; }
        public string Sku { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
