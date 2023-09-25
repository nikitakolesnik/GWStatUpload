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
			Match match = new();
			await _ctx.Matches.AddAsync(match);
			await _ctx.SaveChangesAsync(); // get the generated ID for this match
										   // TODO: reconciling multiple submissions of the same match

			_logger.LogInformation("created new match with ID " + match.Id);


			// insert missing players & create a name:id map

			List<string> submittedPlayerNames = matchEntryDTO.Players
				.Select(x => x.Name)
				.ToList();
			List<Player> playersInTable = _ctx.Players
				.Where(x => submittedPlayerNames.Contains(x.Name))
				.ToList();
			List<Player> playersNotInTable = submittedPlayerNames
				.Except(playersInTable.Select(x => x.Name))
				.Select(x => new Player(x))
				.ToList();
			if (playersNotInTable.Count > 0)
			{
				await _ctx.Players.AddRangeAsync(playersNotInTable);
				await _ctx.SaveChangesAsync(); // get generated IDs for new players
			}
			_logger.LogInformation($"added {playersNotInTable.Count} players to db");
			Dictionary<string, int> playerNameToId = new();
			foreach (Player player in playersInTable.Concat(playersNotInTable))
			{
				playerNameToId.Add(player.Name, player.Id);
			}


			// add the match entry rows
			
			foreach (MatchPlayer player in matchEntryDTO.Players)
			{
				int playerId = playerNameToId[player.Name];
				Team team = Team.Unknown; // figure this out later

				foreach (MatchPlayerSkill skill in player.Skills)
				{
					MatchEntry matchEntry = new(match.Id, team, playerId, skill.Id, skill.Count);
					await _ctx.MatchEntries.AddAsync(matchEntry);
				}
			}
			int rowsAdded = _ctx.SaveChanges();
			_logger.LogInformation($"added {rowsAdded} match entry rows to db");
			return rowsAdded > 0;
		}
	}
}
