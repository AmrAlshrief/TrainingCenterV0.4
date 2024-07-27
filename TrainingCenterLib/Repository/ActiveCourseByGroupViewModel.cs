using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenterLib.Repository
{
    public class ActiveCourseByGroupViewModel

    {
        public int ActiveCourseID { get; set; }
        public int AvailableCourseID { get; set; }
        public string GroupName { get; set; }
        public string GroupDay { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public int TimeSlotID { get; set; }
        public System.TimeSpan StartTime { get; set; }
        public System.TimeSpan EndTime { get; set; }
    }
}
