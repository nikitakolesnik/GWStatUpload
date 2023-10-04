using Microsoft.EntityFrameworkCore;
using StatServer.Contexts;
using StatServer.Entities;
using StatServer.Enums;
using StatServer.Models;

namespace StatServer.Services
{
	public class MatchRepository
	{
		private readonly AppDbContext _ctx;
		private readonly ILogger<MatchRepository> _logger;

		public MatchRepository(AppDbContext ctx, ILogger<MatchRepository> logger)
		{
			_ctx = ctx;
			_logger = logger;
		}

		public IEnumerable<Match> GetAllMatches()
		{
			return _ctx.Matches;
		}

		public IEnumerable<MatchEntry> GetMatchEntriesForMatch(int matchId)
		{
			return _ctx.MatchEntries.Where(x => x.MatchId == matchId);
		}

		public async Task<bool> AddMatchEntry(MatchEntryDTO matchEntryDTO)
		{
			// TODO: reconciling multiple submissions of the same match
			// ...

			Match match = new();
			await _ctx.Matches.AddAsync(match);
			await _ctx.SaveChangesAsync(); // get the generated ID for this match
			
			_logger?.LogInformation("created new match with ID " + match.Id);


			// insert missing players & create a name:id map

			var submittedPlayerNames = matchEntryDTO.Players
				.Select(x => x.Name)
				.ToList();
			var playersInTable = _ctx.Players
				.Where(x => submittedPlayerNames.Contains(x.Name))
				.ToList();
			var playersToInsert = submittedPlayerNames
				.Except(playersInTable.Select(x => x.Name))
				.Select(x => new Player(x))
				.ToList();
			if (playersToInsert.Count > 0)
			{
				await _ctx.Players.AddRangeAsync(playersToInsert);
				await _ctx.SaveChangesAsync(); // get generated IDs for new players
				playersInTable.Concat(playersToInsert);
			}
			_logger?.LogInformation($"added {playersToInsert.Count} players to db");
			
			Dictionary<string, int> playerNameToId = new();
			foreach (Player player in playersInTable)
			{
				playerNameToId.Add(player.Name, player.Id);
			}


			// add the match entry rows
			
			foreach (Player playerEntity in playersInTable)
			{
				MatchPlayer playerDTO = matchEntryDTO.Players.Single(x => x.Name == playerEntity.Name);
				Team team = Team.Unknown; // figure this out later

				foreach (MatchPlayerSkill skill in playerDTO.Skills)
				{
					MatchEntry matchEntry = new(match.Id, team, playerEntity.Id, skill.Id, skill.Count);
					await _ctx.MatchEntries.AddAsync(matchEntry);
				}
			}
			int rowsAdded = _ctx.SaveChanges();
			_logger?.LogInformation($"added {rowsAdded} match entry rows to db");
			return rowsAdded > 0;
		}

		public async Task<IEnumerable<MatchEntryDTO>> GetMatchEntriesForMatches(int offset, int pageSize)
		{
			const int PAGE_SIZE_LIMIT = 50;
			pageSize = Limit(pageSize, 0, PAGE_SIZE_LIMIT);

			List<Match> matches = _ctx.Matches
				.OrderByDescending(m => m.Id)
				.Skip(offset)
				.Take(pageSize)
				.Include(x => x.MatchEntries)
				.ToList();

			// get all match entries for this page of matches in a single DB read
			int[] matchIds = matches.Select(m => m.Id).ToArray();
			List<MatchEntry> matchEntries = _ctx.MatchEntries
				.Include(me => me.Player)
				.Where(me => matchIds.Contains(me.MatchId))
				.ToList();

			List<MatchEntryDTO> result = new();
			foreach (Match match in matches)
			{
				// subset of all match entries for this match only
				List<MatchEntry> matchEntriesForMatch = matchEntries.Where(me => me.MatchId == match.Id).ToList();
				
				// condense the match entry rows into a flat DTO
				MatchEntryDTO matchExport = new(match.Submitted);
				foreach (MatchEntry matchEntry in matchEntriesForMatch)
				{
					string playerName = matchEntry.Player.Name;

					if (!matchExport.Players.Any(x => x.Name == playerName)) // i think it's fine for a prototype, but this is pretty ugly...
					{
						matchExport.Players
							.Add(new MatchPlayer(playerName));
					}

					matchExport.Players
						.Single(x => x.Name == playerName)
						.Skills
						.Add(new MatchPlayerSkill(matchEntry.SkillId, matchEntry.Count));
				}
				result.Add(matchExport);
			}

			return result;
		}

		/// <summary> Keeps an int within a range. Taking 0 to 50: -1 becomes 0, 10 remains unchanged, and 500 becomes 50. </summary>
		private static int Limit(int num, int min, int max) 
		{
			num = Math.Min(num, max);
			num = Math.Max(num, min);
			return num;
		}
	}
}
