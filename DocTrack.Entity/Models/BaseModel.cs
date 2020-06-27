using System;

namespace DocTrack.Entity.Models
{
    public class BaseModel
    {
        public bool IsHidden { get; set; } = false;

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
