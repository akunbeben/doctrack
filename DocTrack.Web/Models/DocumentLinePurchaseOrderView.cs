using System;
using System.ComponentModel.DataAnnotations;

namespace DocTrack.Web.Models
{
    public class DocumentLinePurchaseOrderView
    {
        public int DocumentLineId { get; set; }

        public int DocumentId { get; set; }

        [Required]
        [Display(Name = "Document ID")]
        public string DocumentSequenceNumber { get; set; }

        [Required]
        [Display(Name = "Purchase Order Type")]
        public int PurchaseOrderTypeId { get; set; }

        [Required]
        [Display(Name = "Document Type")]
        public int DocumentTypeId { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required]
        [Display(Name = "Received at MBP")]
        public DateTime ReceivedAtMbp { get; set; }

        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [Display(Name = "Vendor")]
        public int VendorId { get; set; }

        [Required]
        [Display(Name = "Transaction Description")]
        public string TransactionDescription { get; set; }

        [Required]
        [Display(Name = "Purchase Order")]
        public string PoId { get; set; }

        [Required]
        [Display(Name = "TAX Number")]
        public string TaxNumber { get; set; }

        [Required]
        [Display(Name = "DO / CR Number")]
        public string DoCr { get; set; }

        [Required]
        [Display(Name = "Vessel")]
        public int VesselId { get; set; }

        [Required]
        [Display(Name = "Invoice Number")]
        public string DocumentNumber { get; set; }

        [Required]
        [Display(Name = "Invoice Amount")]
        public int InvoiceAmount { get; set; }

        [Required]
        [Display(Name = "TAX Amount: %")]
        public int TaxAmount { get; set; }

        [Required]
        [Display(Name = "Amount to Record")]
        public int AmountToRecord { get; set; }

        [Required]
        [Display(Name = "DP Before Made")]
        public int DpBeforeMade { get; set; }

        [Required]
        [Display(Name = "Others")]
        public string Other { get; set; }

        [Required]
        [Display(Name = "Original Invoice")]
        public bool OriginalInvoice { get; set; }
    }
}