using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocTrack.Web.Models
{
    public class LookupValueView
    {
        public int LookupValueId { get; set; }

        public string Description { get; set; }

        public string LookupType { get; set; }

        public string Abbreviation { get; set; }
    }
}