using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocTrack.Entity.Models
{
    [Table("LookupValue")]
    public class LookupValue
    {
        [Key]
        public int LookupValueId { get; set; }

        public string Description { get; set; }

        public int LookupType { get; set; }

        public string Abbreviation { get; set; }
    }
}
