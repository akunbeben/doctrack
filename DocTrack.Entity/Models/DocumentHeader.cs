using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocTrack.Entity.Models
{
    [Table("DocumentHeader")]
    public class DocumentHeader : BaseModel
    {
        [Key]
        public int DocumentId { get; set; }

        public string DocumentSequenceNumber { get; set; }

        public int DocumentTypeId { get; set; }

        public int? VendorId { get; set; }

        public int? Amount { get; set; }

        public string DocumentStatus { get; set; }

        public string FlowStatus { get; set; }
    }
}