using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocTrack.Entity.Dto
{
    public class DocumentTypeDto
    {
        public int DocumentTypeId { get; set; }

        public string DocumentTypeName { get; set; }

        public int FormType { get; set; }

        public string PoolName { get; set; }

        public string FormName { get; set; }

        public string DocumentReferenceName { get; set; }
    }
}
