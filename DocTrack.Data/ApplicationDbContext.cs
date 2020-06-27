using DocTrack.Common;
using DocTrack.Entity.Models;
using System.Data.Entity;

namespace DocTrack.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base(GlobalNamespace.ConnectionString) { }

        public virtual DbSet<SequenceNumber> SequenceNumber { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<PersonInCharge> PersonInCharge { get; set; }
        public virtual DbSet<DocumentType> DocumentType { get; set; }
        public virtual DbSet<LookupValue> LookupValue { get; set; }
        public virtual DbSet<DocumentHeader> DocumentHeader { get; set; }
        public virtual DbSet<DocumentLinePurchaseOrder> DocumentLinePurchaseOrder { get; set; }
        public virtual DbSet<DocumentLineCashAdvance> DocumentLineCashAdvance { get; set; }
        public virtual DbSet<DocumentLineMemo> DocumentLineMemo { get; set; }
        public virtual DbSet<DocumentLineSettlement> DocumentLineSettlement { get; set; }
        public virtual DbSet<UtilityDocument> UtilityDocument { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //
        }
    }
}
