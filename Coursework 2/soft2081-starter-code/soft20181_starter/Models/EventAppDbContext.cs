using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace soft20181_starter.Models
{
    public class EventAppDbContext : IdentityDbContext<UsersInfo>
    {
        public EventAppDbContext(DbContextOptions<EventAppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Contact> ContactTable{ get; set; }
    }

}

