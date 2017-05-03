using CompSpyWeb.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CompSpyWeb.DAL
{
    public class CompSpyContext : DbContext
    {
        public CompSpyContext() : base("CompSpyContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<ClassroomPermission> ClassroomPermissions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<User>()
                .HasOptional(u => u.Creator)
                .WithMany()
                .HasForeignKey(u => u.CreatorID);

            modelBuilder.Entity<User>()
                .HasOptional(u => u.Editor)
                .WithMany()
                .HasForeignKey(u => u.EditorID);
        }
    }
}