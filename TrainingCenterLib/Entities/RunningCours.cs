//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainingCenterLib.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class RunningCours
    {
        public int RunningCourseID { get; set; }
        public int WaitingListID { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public int UserID { get; set; }
        public string AvailableCourseID { get; set; }
    
        public virtual WaitingList WaitingList { get; set; }
    }
}
