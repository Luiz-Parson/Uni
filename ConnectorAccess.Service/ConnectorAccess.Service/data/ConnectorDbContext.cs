

using ConnectorAccess.Service.models;
using Microsoft.EntityFrameworkCore;

namespace ConnectorAccess.Service.data
{
    public class ConnectorDbContext : DbContext
    {
        public ConnectorDbContext(DbContextOptions<ConnectorDbContext> options) : base(options) { }

        public DbSet<AccessControl> AccessControl { get; set; }
        public DbSet<GeneralControl> GeneralControl { get; set; }
        public DbSet<ExclusionControl> ExclusionControl { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<SystemUser> SystemUser { get; set; }
    }
}
