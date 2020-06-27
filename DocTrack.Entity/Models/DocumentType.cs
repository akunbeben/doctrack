using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocTrack.Entity.Models
{
    [Table("DocumentType")]
    public class DocumentType : BaseModel
    {
        [Key]
        public int DocumentTypeId { get; set; }

        public string DocumentTypeName { get; set; }

        public int FormType { get; set; }

        public string PoolName { get; set; }

        public int? DocumentReference { get; set; }
    }
}
