﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainingCenterUI.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TrainingCenterUIDbContext : DbContext
    {
        public TrainingCenterUIDbContext()
            : base("name=TrainingCenterUIDbContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AuditTrailView> AuditTrailViews { get; set; }
        public virtual DbSet<AvailableCouresView> AvailableCouresViews { get; set; }
        public virtual DbSet<InstructorsAvailabilityView> InstructorsAvailabilityViews { get; set; }
        public virtual DbSet<WaitingListView> WaitingListViews { get; set; }
        public virtual DbSet<AuditTrailView2> AuditTrailViews2 { get; set; }
    }
}
