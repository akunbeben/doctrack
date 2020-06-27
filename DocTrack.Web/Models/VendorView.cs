using System.ComponentModel.DataAnnotations;

namespace DocTrack.Web.Models
{
    public class VendorView
    {
        public int VendorId { get; set; }

        [Display(Name = "Vendor Name")]
        [Required]
        public string VendorName { get; set; }

        [Display(Name = "Vendor Own")]
        [Required]
        public int VendorOwn { get; set; }

        [Display(Name = "Vendor Account")]
        [Required]
        public string VendorAccount { get; set; }

        [Display(Name = "Payment Terms")]
        [Required]
        public int PaymentTerms { get; set; }
    }
}