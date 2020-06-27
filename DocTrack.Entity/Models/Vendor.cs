using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocTrack.Entity.Models
{
    [Table("Vendor")]
    public class Vendor : BaseModel
    {
        [Key]
        public int VendorId { get; set; }

        public string VendorName { get; set; }

        public int VendorOwn { get; set; }

        public string VendorAccount { get; set; }

        public int PaymentTerms { get; set; }
    }
}