using DocTrack.Web.Models;
using System.Web.Mvc;

namespace DocTrack.Web.ViewModel
{
    public class DocumentTypeVM
    {
        public DocumentTypeView DocumentType { get; set; }

        public SelectList FormType { get; set; }

        public SelectList DocumentReference { get; set; }
    }
}