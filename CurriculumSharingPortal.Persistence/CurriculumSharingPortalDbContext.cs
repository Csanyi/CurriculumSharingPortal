using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CurriculumSharingPortal.Persistence
{
	public class CurriculumSharingPortalDbContext : IdentityDbContext<IdentityUser>
    {
        public CurriculumSharingPortalDbContext(DbContextOptions<CurriculumSharingPortalDbContext> options) : base(options) { }

        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Curriculum> Curriculums { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<User> Members { get; set; } = null!;
        public DbSet<Admin> Admins { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>(entity => { entity.ToTable("Members"); });
            builder.Entity<Admin>(entity => { entity.ToTable("Admins"); });
        }
    }
}
