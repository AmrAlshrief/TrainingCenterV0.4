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
    
    public partial class MasterEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MasterEntity()
        {
            this.AuditTrails = new HashSet<AuditTrail>();
        }
    
        public int EntityID { get; set; }
        public string EntityName { get; set; }
        public string DisplayName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuditTrail> AuditTrails { get; set; }
    }
}
