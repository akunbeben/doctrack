using DocTrack.Web.Models;
using System.Web.Mvc;

namespace DocTrack.Web.ViewModel
{
    public class DocumentLineCashAdvanceVM
    {
        public DocumentLineCashAdvanceView CashAdvanceView { get; set; }

        public SelectList Companies { get; set; }

        public SelectList Departments { get; set; }

        public SelectList Vendors { get; set; }

        public SelectList DocumentStatus { get; set; }
    }
}