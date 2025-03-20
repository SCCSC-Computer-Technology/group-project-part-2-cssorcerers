/*
 * GROUP PROJECT #2
 * APPLICATION DB CONTEXT
 */

using Microsoft.EntityFrameworkCore;
using SportIQ.Models;

namespace SportIQ.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
