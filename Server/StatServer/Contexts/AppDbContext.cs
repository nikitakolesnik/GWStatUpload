using Microsoft.EntityFrameworkCore;
using StatServer.Entities;

namespace StatServer.Contexts
{
	public class AppDbContext : DbContext
	{
		private ILogger<AppDbContext> _logger;

		public DbSet<Match> Matches { get; set; }
		public DbSet<MatchEntry> MatchEntries { get; set; }
		public DbSet<Player> Players { get; set; }

		public AppDbContext(ILogger<AppDbContext> logger)
		{
			_logger = logger;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			const string connStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StatServerDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
			optionsBuilder.UseSqlServer(connStr);
			_logger?.LogInformation("Made db connection");
		}
	}
}
