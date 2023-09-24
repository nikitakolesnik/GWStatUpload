using Microsoft.EntityFrameworkCore;
using StatServer.Entities;

namespace StatServer.Contexts
{
	internal class AppDbContext : DbContext
	{
		public DbSet<Match> Matches { get; set; }
		public DbSet<MatchEntry> MatchEntries { get; set; }
		public DbSet<Player> Players { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			const string connStr1 = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StatServerDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
			optionsBuilder.UseSqlServer(connStr1);
		}
	}
}
