using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocTrack.Entity.Models
{
    [Table("SequenceNumber")]
    public class SequenceNumber
    {
        [Key]
        public int DocSequenceId { get; set; }

        public string DocSequenceName { get; set; }

        public int DocSequenceNumber { get; set; }

        public string DocSequenceFormat { get; set; }
    }
}
