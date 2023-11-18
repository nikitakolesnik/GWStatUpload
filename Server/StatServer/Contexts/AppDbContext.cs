using Microsoft.EntityFrameworkCore;
using StatServer.Data;
using StatServer.Entities;
using StatServer.Enums;
using System.Collections.Generic;

namespace StatServer.Contexts
{
	public class AppDbContext : DbContext
	{
		private readonly ILogger<AppDbContext> _logger;
		private readonly Random _random = new();

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
			const string connStr = @"Data Source=(localdb)\MSSQLLocalDB;" +
				"Initial Catalog=StatServerDB;" +
				"Integrated Security=True;" +
				"Connect Timeout=30;" +
				"Encrypt=False;" +
				"Trust Server Certificate=False;" +
				"Application Intent=ReadWrite;" +
				"Multi Subnet Failover=False";
			optionsBuilder.UseSqlServer(connStr);
			_logger?.LogInformation("Made db connection");
		}

		protected override async void OnModelCreating(ModelBuilder modelBuilder)
		{
			//modelBuilder.Entity<Player>().HasKey(x => x.Id);
			//modelBuilder.Entity<Match>().HasKey(x => x.Id);
			//modelBuilder.Entity<MatchEntry>().HasKey(x => x.Id);

			// initialize fake players
			string[] fakeNames =
			{
				"Princess Davenport", "Dariel McCarthy", "Kira Ortiz", "Landon Rice", "Ada Good", "Davian Arias",
				"Aleah Huffman", "Chris Leonard", "Demi Dickson", "Maxton Valenzuela", "Henley Gomez",
				"Isaiah McIntosh", "Gwen Wolf", "Jase Howard", "Sophie Villa", "Clay Cooper", "Serenity Nava",
				"Stefan Bruce", "Marilyn Stanley", "Manuel McConnell", "Denise Novak", "Bishop Friedman",
				"Aspyn Hammond", "Francis Barber", "Cassidy Santos", "Walker Douglas", "Aniyah Winters",
				"Deandre Carroll", "Zara Andrade", "Abdiel Cooper", "Serenity Mullen", "Shepard Pittman", "Marie Lamb",
				"Kaysen Watkins", "Lola Villalobos", "Reuben Trevino", "Priscilla Wallace", "Chase Ahmed",
				"Jolie Magana", "Rey Lindsey", "Colette Berry", "Adonis Henry", "Summer Avila", "Jaylen Bowman",
				"Fiona Ingram", "Tripp Krueger", "Kamari Poole", "Quincy Rowe", "Matilda Wade", "Jake Michael",
				"Aubriella Garrison", "Noe Heath", "Amani Gallegos", "Jonas Graves", "Elle Stanley", "Manuel Phelps",
				"Laney Rosales", "Wilder Stanton", "Jaycee Hernandez", "Mason McCullough", "Hana Gould", "Blaine Mann",
				"Paislee Harvey", "Cayden O’Connor", "Charli Hutchinson", "Korbin Villanueva", "Monroe McDowell",
				"Lachlan McDonald", "Daisy Villalobos", "Reuben Castillo", "Eva Pollard", "Jad Winters",
				"Kataleya Blankenship", "Ernesto Dillon", "Laurel Magana", "Rey Nguyen", "Nova Morton", "Roland Fuller",
				"Oakley Joseph", "Kyle Patterson"
			};
			List<Player> players = new();
			for (int i = 0; i < fakeNames.Length; i++)
			{
				int playerId = (i + 1) * -1;
				players.Add(new Player(playerId, $"TEST {fakeNames[i]}"));
			}
			modelBuilder.Entity<Player>().HasData(players);

			// each match, 8 vs 8 players each cast ? diff unique skills ?? diff times
			List<int> skillIds = Skills.Map.Keys.Skip(1).ToList(); // skip the "no skill" entry
			const int numFakeMatchesToCreate = 150;
			const int maxUsesOfSkill = 50;
			int matchEntryIdCounter = -1;
			for (int i = 1; i <= numFakeMatchesToCreate; i++)
			{
				// randomly pick which team won
				int randomTeamChoice = _random.Next(0, 3); // high likelyhood just for UI testing
				Team winningTeam = randomTeamChoice switch
				{
					0 => Team.Draw,
					1 => Team.Blue,
					_ => Team.Red
				};

				// create match entity
				int matchId = i * -1;
				Match match = new(matchId, winningTeam);
				modelBuilder.Entity<Match>().HasData(match);

				// create teams for match entry calculation
				List<int> randomIndices = GetXRandomUniqueIndices(16, players.Count);
				List<Player> redTeam = randomIndices.Take(8).Select(index => players[index]).ToList();
				List<Player> blueTeam = randomIndices.Skip(8).Take(8).Select(index => players[index]).ToList();

				// create match entry rows (skill use per player)
				void CreateMatchEntryRows(Team team, List<Player> players)
				{
					foreach (Player player in players)
					{
						// get 8 random skills for the player to have used (don't care about profession)
						// skill IDs aren't consecutive; translate random index to the random-ish IDs

						List<int> skillsUsedIndices = GetXRandomUniqueIndices(8, skillIds.Count);
						foreach (int skillIdIndex in skillsUsedIndices)
						{
							int skillId = skillIds[skillIdIndex];
							int skillUsedCount = _random.Next(maxUsesOfSkill);
							MatchEntry matchEntry = new(matchEntryIdCounter--, match.Id, team, player.Id, skillId, skillUsedCount);
							modelBuilder.Entity<MatchEntry>().HasData(matchEntry);
						}
					}
				}
				CreateMatchEntryRows(Team.Red, redTeam);
				CreateMatchEntryRows(Team.Blue, blueTeam);
			}
		}

		private List<int> GetXRandomUniqueIndices(int count, int maxValue)
		{
			HashSet<int> randomIndices = new();

			while (randomIndices.Count < count)
			{
				randomIndices.Add(_random.Next(maxValue));
			}

			return randomIndices.ToList();
		}
	}
}
