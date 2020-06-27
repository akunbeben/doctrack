using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DocTrack.Web.Models
{
    public class DepartmentView
    {
        public int DepartmentId { get; set; }

        [Display(Name = "Department Number")]
        [Required]
        public string DepartmentNumber { get; set; }

        [Display(Name = "Department Name")]
        [Required]
        public string DepartmentName { get; set; }
    }
}