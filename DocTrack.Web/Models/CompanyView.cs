using System.ComponentModel.DataAnnotations;

namespace DocTrack.Web.Models
{
    public class CompanyView
    {
        public int CompanyId { get; set; }

        [Display(Name = "Company Number")]
        [Required]
        public string CompanyNumber { get; set; }

        [Display(Name = "Company Name")]
        [Required]
        public string CompanyName { get; set; }
    }
}