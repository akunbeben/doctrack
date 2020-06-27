using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DocTrack.Web.Models
{
    public class DocumentLineSettlementView
    {
        public int DocumentLineId { get; set; }

        public int DocumentId { get; set; }

        [Required]
        [Display(Name = "Document ID")]
        public string DocumentSequenceNumber { get; set; }

        [Required]
        [Display(Name = "Document Type")]
        public int DocumentTypeId { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; } // Dropdown

        [Required]
        [Display(Name = "Received at MBP")]
        public DateTime ReceivedAtMbp { get; set; }

        [Required]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Required]
        [Display(Name = "Vendor")]
        public int VendorId { get; set; } // Dropdown

        [Required]
        [Display(Name = "Transaction Description")]
        public string TransactionDescription { get; set; }

        [Required]
        [Display(Name = "Tax Number")]
        public string TaxNumber { get; set; }

        [Required]
        [Display(Name = "Document Date")]
        public DateTime DocumentDate { get; set; }

        [Required]
        [Display(Name = "Document Number")]
        public string DocumentNumber { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public int Amount { get; set; }

        [Required]
        [Display(Name = "Tax Amount: %")]
        public int TaxAmount { get; set; }

        [Required]
        [Display(Name = "Total Amount")]
        public int TotalAmount { get; set; }

        [Required]
        [Display(Name = "Document Status")]
        public int DocumentStatus { get; set; }

        [Display(Name = "Document Reference")]
        public string DocumentReference { get; set; }
    }
}