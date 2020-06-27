using DocTrack.Web.Models;
using System.Web.Mvc;


namespace DocTrack.Web.ViewModel
{
    public class VendorVM
    {
        public VendorView Vendors { get; set; }
        public SelectList Companies { get; set; }
    }
}