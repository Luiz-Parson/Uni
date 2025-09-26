using System;

namespace ConnectorAccess.Service.models
{
    public class Product
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public string EPC { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
