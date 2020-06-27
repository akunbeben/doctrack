using DocTrack.Web.Models;
using System.Web.Mvc;

namespace DocTrack.Web.ViewModel
{
    public class PersonInChargeVM
    {
        public PersonInChargeView PersonInCharge { get; set; }

        public SelectList Departments { get; set; }
    }
}