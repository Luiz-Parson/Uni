using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectorAccess.models
{
    class ExclusionControlDTO
    {
        public int ProductId { get; set; }
        public string Category { get; set; }
        public DateTime ExcludedOn { get; set; }
    }
}
