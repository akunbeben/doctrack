using DocTrack.Web.Models;
using System.Web.Mvc;

namespace DocTrack.Web.ViewModel
{
    public class DocumentLinePurchaseOrderVM
    {
        public DocumentLinePurchaseOrderView PurchaseOrderView { get; set; }

        public SelectList Companies { get; set; }

        public SelectList Vendors { get; set; }

        public SelectList PurchaseOrderType { get; set; }
    }
}