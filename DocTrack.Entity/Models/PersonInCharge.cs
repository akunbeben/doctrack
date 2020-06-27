using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocTrack.Entity.Models
{
    [Table("PersonInCharge")]
    public class PersonInCharge : BaseModel
    {
        [Key]
        public int PersonInChargeId { get; set; }

        public string PersonInChargeName { get; set; }

        public int DepartmentId { get; set; }
    }
}
