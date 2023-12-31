using Microsoft.EntityFrameworkCore;

namespace Db
{
    internal class PortfolioContext : DbContext
    {
        public DbSet<Thing> Things => Set<Thing>();

        private string _path;

        public PortfolioContext(string path)
        {
            _path = path;

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_path}");
        }
    }
}
