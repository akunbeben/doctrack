using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocTrack.Entity.Models
{
    [Table("Users")]
    public class Users : BaseModel
    {
        [Key]
        public int UserId { get; set; }

        public string Username { get; set; }

        public int UserRole { get; set; }

        public int DepartmentId { get; set; }

        public int CompanyId { get; set; }
    }
}
