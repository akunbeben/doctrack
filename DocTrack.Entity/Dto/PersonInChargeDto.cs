using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocTrack.Entity.Dto
{
    public class PersonInChargeDto
    {
        public int PersonInChargeId { get; set; }

        public string PersonInChargeName { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }
    }
}
