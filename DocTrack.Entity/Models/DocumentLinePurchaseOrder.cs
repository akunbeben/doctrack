using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocTrack.Entity.Models
{
    [Table("DocumentLinePurchaseOrder")]
    public class DocumentLinePurchaseOrder
    {
        [Key]
        public int DocumentLineId { get; set; }

        public int DocumentId { get; set; }

        public string DocumentSequenceNumber { get; set; }

        public int PurchaseOrderTypeId { get; set; }

        public int DocumentTypeId { get; set; }

        public int CompanyId { get; set; }

        public DateTime ReceivedAtMbp { get; set; }

        public DateTime InvoiceDate { get; set; }

        public int VendorId { get; set; }

        public string TransactionDescription { get; set; }

        public string PoId { get; set; }

        public string TaxNumber { get; set; }

        public string DoCr { get; set; }

        public int VesselId { get; set; }

        public string DocumentNumber { get; set; }

        public int InvoiceAmount { get; set; }

        public int TaxAmount { get; set; }

        public int AmountToRecord { get; set; }

        public int DpBeforeMade { get; set; }

        public string Other { get; set; }

        public bool OriginalInvoice { get; set; }
    }
}
