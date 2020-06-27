using DocTrack.Web.Models;
using System.Web.Mvc;

namespace DocTrack.Web.ViewModel
{
    public class DocumentHeaderVM
    {
        public DocumentHeaderView DocumentHeader { get; set; }

        public SelectList DocumentType { get; set; }
    }
}