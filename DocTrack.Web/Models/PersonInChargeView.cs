using System.ComponentModel.DataAnnotations;

namespace DocTrack.Web.Models
{
    public class PersonInChargeView
    {
        public int PersonInChargeId { get; set; }

        [Required]
        [Display(Name = "Person in Charge Name")]
        public string PersonInChargeName { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
    }
}