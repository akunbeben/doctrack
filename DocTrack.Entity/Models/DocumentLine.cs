namespace DocTrack.Entity.Models
{
    public class DocumentLine
    {
        public int DocumentId { get; set; }

        public string DocumentSequenceNumber { get; set; }

        public string DocumentTypeName { get; set; }

        public string VendorName { get; set; }

        public int? Amount { get; set; }

        public string DocumentStatus { get; set; }

        public string FlowStatus { get; set; }
    }
}
