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
    
    public partial class AuditTrail
    {
        public int AuditTrailID { get; set; }
        public int ActionTypeID { get; set; }
        public int ActionID { get; set; }
        public int UserID { get; set; }
        public string IpAddress { get; set; }
        public System.DateTime TransactionTime { get; set; }
        public int EntityID { get; set; }
        public string EntityRecord { get; set; }
    
        public virtual Action Action { get; set; }
        public virtual ActionType ActionType { get; set; }
        public virtual MasterEntity MasterEntity { get; set; }
    }
}
