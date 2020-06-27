using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocTrack.Entity.Models
{
    [Table("Department")]
    public class Department : BaseModel
    {
        [Key]
        public int DepartmentId { get; set; }

        public string DepartmentNumber { get; set; }

        public string DepartmentName { get; set; }
    }
}
