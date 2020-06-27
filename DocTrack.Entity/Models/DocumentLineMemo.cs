using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocTrack.Entity.Models
{
    [Table("DocumentLineMemo")]
    public class DocumentLineMemo
    {
        [Key]
        public int DocumentLineId { get; set; }

        public int DocumentId { get; set; }

        public string DocumentSequenceNumber { get; set; }

        public int DocumentTypeId { get; set; }

        public int CompanyId { get; set; }

        public int DepartmentId { get; set; }

        public DateTime ReceivedAtMbp { get; set; }

        public DateTime DueDate { get; set; }

        public int VendorId { get; set; }

        public string TransactionDescription { get; set; }

        public string TaxNumber { get; set; }

        public DateTime DocumentDate { get; set; }

        public string DocumentNumber { get; set; }

        public int Amount { get; set; }

        public int TaxAmount { get; set; }

        public int TotalAmount { get; set; }

        public int DocumentStatus { get; set; }
    }
}
