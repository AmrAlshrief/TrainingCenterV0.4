using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainingCenterUI.Models
{
    public class WaitingListViewModel
    {
        public int StudentID { get; set; }
        public int AvailableCourseID { get; set; }
        public string GroupName { get; set; }
        public bool IsPaid { get; set; }
        public bool IsCash { get; set; }

        public IEnumerable<SelectListItem> Students { get; set; }
        public IEnumerable<SelectListItem> Courses { get; set; }
    }
}