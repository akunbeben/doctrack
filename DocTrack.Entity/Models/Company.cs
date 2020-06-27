using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocTrack.Entity.Models
{
    [Table("Company")]
    public class Company : BaseModel
    {
        [Key]
        public int CompanyId { get; set; }

        public string CompanyNumber { get; set; }

        public string CompanyName { get; set; }
    }
}
