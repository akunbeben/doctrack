using System.ComponentModel.DataAnnotations;

namespace DocTrack.Web.Models
{
    public class FormTypeView
    {
        public int LookupValueId { get; set; }

        [Required]
        [Display(Name = "Form Name")]
        public string FormName { get; set; }
    }
}