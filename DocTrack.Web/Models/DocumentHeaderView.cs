using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DocTrack.Web.Models
{
    public class DocumentHeaderView
    {
        public int DocumentId { get; set; }

        [Required]
        [Display(Name = "Document Number")]
        public string DocumentSequenceNumber { get; set; }

        [Required]
        [Display(Name = "Document Type")]
        public int DocumentTypeId { get; set; }

        public int? VendorId { get; set; }

        public int? Amount { get; set; }

        public string DocumentStatus { get; set; }

        public string FlowStatus { get; set; }
    }
}