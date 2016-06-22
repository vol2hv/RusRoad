namespace RusRoadLib
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RusRoadsData : DbContext
    {
        public RusRoadsData() : base("name=RusRoadsData") { }

        public virtual DbSet<CarOwner> CarOwner { get; set; }
        public virtual DbSet<Highway> Highway { get; set; }
        public virtual DbSet<Passage> Passage { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarOwner>()
                .HasMany(e => e.Passage)
                .WithRequired(e => e.CarOwner)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Highway>()
                .HasMany(e => e.Passage)
                .WithRequired(e => e.Highway)
                .WillCascadeOnDelete(false);
        }
    }
}
