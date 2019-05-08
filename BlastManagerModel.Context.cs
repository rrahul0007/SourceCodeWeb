﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlastManager.Service.Main.Backend.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using Qilin.Service.Server.DataAccess;
    using BlastManager.Service.Main.Common.BusinessEntity;
    public partial class BlastManagerModelContainer : BaseDbContext
    {
        public BlastManagerModelContainer()
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            // Timeout 3 mins
            this.Database.CommandTimeout = 180;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AuditLogDetail> AuditLogDetails { get; set; }
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public virtual DbSet<BlastingEngineer> BlastingEngineers { get; set; }
        public virtual DbSet<BlastProject> BlastProjects { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<CodeListInternal> CodeListInternals { get; set; }
        public virtual DbSet<CodePage> CodePages { get; set; }
        public virtual DbSet<CostsDetonator> CostsDetonators { get; set; }
        public virtual DbSet<CostsDualDelay> CostsDualDelays { get; set; }
        public virtual DbSet<CostsEx> CostsExes { get; set; }
        public virtual DbSet<Costsretarder> Costsretarders { get; set; }
        public virtual DbSet<DBVersion> DBVersions { get; set; }
        public virtual DbSet<Detonator> Detonators { get; set; }
        public virtual DbSet<DetonatorType> DetonatorTypes { get; set; }
        public virtual DbSet<DetonatorTypeC> DetonatorTypeCs { get; set; }
        public virtual DbSet<DetonatorTypeC_EN> DetonatorTypeC_EN { get; set; }
        public virtual DbSet<Driller> Drillers { get; set; }
        public virtual DbSet<DrillRigContractor> DrillRigContractors { get; set; }
        public virtual DbSet<DrillRig> DrillRigs { get; set; }
        public virtual DbSet<DualDelay> DualDelays { get; set; }
        public virtual DbSet<ExGrößen> ExGrößen { get; set; }
        public virtual DbSet<Explosive> Explosives { get; set; }
        public virtual DbSet<ExType> ExTypes { get; set; }
        public virtual DbSet<Lager> Lagers { get; set; }
        public virtual DbSet<Lieferant> Lieferants { get; set; }
        public virtual DbSet<Manufact> Manufacts { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<Organisation> Organisations { get; set; }
        public virtual DbSet<PasswordHistory> PasswordHistories { get; set; }
        public virtual DbSet<PricingInvoice> PricingInvoices { get; set; }
        public virtual DbSet<Quarry> Quarries { get; set; }
        public virtual DbSet<Retarder> Retarders { get; set; }
        public virtual DbSet<RetarderDelay> RetarderDelays { get; set; }
        public virtual DbSet<RetarderTypeC> RetarderTypeCs { get; set; }
        public virtual DbSet<RetarderTypeC_EN> RetarderTypeC_EN { get; set; }
        public virtual DbSet<SiteMap> SiteMaps { get; set; }
        public virtual DbSet<Sprenghelfer> Sprenghelfers { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<Surveyor> Surveyors { get; set; }
        public virtual DbSet<SystemType> SystemTypes { get; set; }
        public virtual DbSet<UserActivity> UserActivities { get; set; }
        public virtual DbSet<UserLogActivity> UserLogActivities { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VibMeasurement> VibMeasurements { get; set; }
        public virtual DbSet<VibrationMeasDev> VibrationMeasDevs { get; set; }
        public virtual DbSet<VibrationMeasurementSite> VibrationMeasurementSites { get; set; }
        public virtual DbSet<SyncCredential> SyncCredentials { get; set; }
    }
}
