using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocTrack.Entity.Models
{
    [Table("UtilityDocument")]
    public class UtilityDocument
    {
        [Key]
        public int Id { get; set; }

        public int DocumentTypeId { get; set; }

        public string RedirectTo { get; set; }
    }
}
