using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace soft20181_starter.Models
{
    public class EventAppDbContext : IdentityDbContext<UsersInfo>
    {
        public EventAppDbContext(DbContextOptions<EventAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Contact> ContactTable { get; set; }
        public DbSet<UserEventRegistration> userEventRegistrations { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserEventRegistration>().HasKey(a => new {a.UserId, a.EventId });
            // Configure the Event entity
            builder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Location).HasMaxLength(100);
                entity.Property(e => e.Category)  // Add this
                               .HasMaxLength(50)
                               .IsRequired(false);
                entity.Property(e => e.Image).HasMaxLength(100);
            });
        }
    }
}