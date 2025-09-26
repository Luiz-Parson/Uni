using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectorAccess.models
{
    class DeleteMultipleDTO
    {
        public List<int> Ids { get; set; }
        public string DeletedBy { get; set; }
    }
}
