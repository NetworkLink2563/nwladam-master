﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NWL_AdAm_DML
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class AdAmEntities : DbContext
    {
        public AdAmEntities()
            : base("name=AdAmEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TMstMController> TMstMController { get; set; }
        public virtual DbSet<TMstMCustomer> TMstMCustomer { get; set; }
        public virtual DbSet<TMstMLamp> TMstMLamp { get; set; }
        public virtual DbSet<TMstMProject> TMstMProject { get; set; }
        public virtual DbSet<TMstMUser> TMstMUser { get; set; }
        public virtual DbSet<TSysSDistrict> TSysSDistrict { get; set; }
        public virtual DbSet<TSysSProvince> TSysSProvince { get; set; }
        public virtual DbSet<TSysSSubDistrict> TSysSSubDistrict { get; set; }
        public virtual DbSet<TSysSConfig> TSysSConfig { get; set; }
        public virtual DbSet<TTrnTLampUpdate> TTrnTLampUpdate { get; set; }
        public virtual DbSet<TTrnTWarning> TTrnTWarning { get; set; }
        public virtual DbSet<V_LampStatus> V_LampStatus { get; set; }
        public virtual DbSet<TDocTJob> TDocTJob { get; set; }
    
        public virtual ObjectResult<STP_NWLShowJobStatus_Result> STP_NWLShowJobStatus(string ptCstCode, string ptPrjCode)
        {
            var ptCstCodeParameter = ptCstCode != null ?
                new ObjectParameter("ptCstCode", ptCstCode) :
                new ObjectParameter("ptCstCode", typeof(string));
    
            var ptPrjCodeParameter = ptPrjCode != null ?
                new ObjectParameter("ptPrjCode", ptPrjCode) :
                new ObjectParameter("ptPrjCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STP_NWLShowJobStatus_Result>("STP_NWLShowJobStatus", ptCstCodeParameter, ptPrjCodeParameter);
        }
    
        public virtual ObjectResult<STP_NWLShowOnOffStatus_Result> STP_NWLShowOnOffStatus(string ptCstCode, string ptPrjCode)
        {
            var ptCstCodeParameter = ptCstCode != null ?
                new ObjectParameter("ptCstCode", ptCstCode) :
                new ObjectParameter("ptCstCode", typeof(string));
    
            var ptPrjCodeParameter = ptPrjCode != null ?
                new ObjectParameter("ptPrjCode", ptPrjCode) :
                new ObjectParameter("ptPrjCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STP_NWLShowOnOffStatus_Result>("STP_NWLShowOnOffStatus", ptCstCodeParameter, ptPrjCodeParameter);
        }
    
        public virtual ObjectResult<STP_NWLShowEMMStatus_Result> STP_NWLShowEMMStatus(string ptCstCode, string ptPrjCode)
        {
            var ptCstCodeParameter = ptCstCode != null ?
                new ObjectParameter("ptCstCode", ptCstCode) :
                new ObjectParameter("ptCstCode", typeof(string));
    
            var ptPrjCodeParameter = ptPrjCode != null ?
                new ObjectParameter("ptPrjCode", ptPrjCode) :
                new ObjectParameter("ptPrjCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STP_NWLShowEMMStatus_Result>("STP_NWLShowEMMStatus", ptCstCodeParameter, ptPrjCodeParameter);
        }
    }
}
