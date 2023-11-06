using Microsoft.EntityFrameworkCore;
using StatServer.Data;
using StatServer.Entities;
using StatServer.Enums;

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
			const string connStr = @"Data Source=(localdb)\MSSQLLocalDB;"+
				"Initial Catalog=StatServerDB;"+
				"Integrated Security=True;"+
				"Connect Timeout=30;"+
				"Encrypt=False;"+
				"Trust Server Certificate=False;"+
				"Application Intent=ReadWrite;"+
				"Multi Subnet Failover=False";
			optionsBuilder.UseSqlServer(connStr);
			_logger?.LogInformation("Made db connection");
		}

		protected override async void OnModelCreating(ModelBuilder modelBuilder)
		{
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
			Players.AddRange(fakeNames.Select(name => new Player($"TEST {name}")));
			await SaveChangesAsync();

			// each match, 8 vs 8 players each cast ? diff unique skills ?? diff times
			List<int> skillIds = Skills.Map.Keys.Skip(1).ToList(); // skip the "no skill" entry
			const int numFakeMatchesToCreate = 150;
			const int maxUsesOfSkill = 50;
			for (int i = 0; i < numFakeMatchesToCreate; i++)
			{
				// randomly pick which team won
				int randomTeamChoice = _random.Next(3);
				Team winningTeam = randomTeamChoice switch
				{
					0 => Team.Draw,
					1 => Team.Blue,
					_ => Team.Red
				};

				// create and save the match, to get a match id
				Match match = new();
				await Matches.AddAsync(match);
				await SaveChangesAsync();
				
				// create teams
				List<int> randomIndices = GetXRandomUniqueIndices(16, Players.Count());
				List<Player> redTeam  = Players.Where(x => randomIndices.Take(8).Contains(x.Id)).ToList();
				List<Player> blueTeam = Players.Where(x => randomIndices.Skip(8).Take(8).Contains(x.Id)).ToList();
				
				// create match entry rows (skill use per player)
				foreach (Player player in redTeam)
				{
					// get 8 random skills for the player to have used (don't care about profession)
					// skill IDs aren't consecutive; translate random index to the random-ish IDs
					List<int> skillsUsedIndices = GetXRandomUniqueIndices(8, Skills.Map.Count);
					List<int> skillsUsedIds = new(8);
					for (int j = 0; j < skillsUsedIndices.Count(); j++)
					{
						skillsUsedIds[j] = skillIds[skillsUsedIndices[j]];
					}
					
					// create
					foreach (int skillId in skillsUsedIds)
					{
						int skillUsedCount = _random.Next(maxUsesOfSkill);
						await MatchEntries.AddAsync(new MatchEntry(match.Id, Team.Red, player.Id, skillId, skillUsedCount));
					}
				}
			}
			await SaveChangesAsync();
		}

		private List<int> GetXRandomUniqueIndices(int num, int maxValue)
		{
			HashSet<int> randomIndices = new();

			while (randomIndices.Count < num)
			{
				randomIndices.Add(_random.Next(maxValue));
			}

			return randomIndices.ToList();
		}
	}
}
