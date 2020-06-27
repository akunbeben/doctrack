using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocTrack.Entity.Dto
{
    public class VendorDto
    {
        public int VendorId { get; set; }

        public string VendorName { get; set; }

        public int VendorOwn { get; set; }

        public string CompanyName { get; set; }

        public string VendorAccount { get; set; }

        public int PaymentTerms { get; set; }
    }
}
