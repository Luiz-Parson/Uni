using System.Collections.Generic;

namespace ConnectorAccess.Service.models.dtos
{
    public class DeleteMultipleDTO
    {
        public List<int> Ids { get; set; }
        public string DeletedBy { get; set; }
    }
}
