using System.ComponentModel.DataAnnotations;

namespace DocTrack.Web.Models
{
    public class DocumentTypeView
    {
        public int DocumentTypeId { get; set; }

        [Required]
        [Display(Name = "Document Type Name")]
        public string DocumentTypeName { get; set; }

        [Required]
        [Display(Name = "Form Type")]
        public int FormType { get; set; }

        [Required]
        [Display(Name = "Pool Name")]
        public string PoolName { get; set; }

        [Display(Name = "Document Reference")]
        public int? DocumentReference { get; set; }
    }
}