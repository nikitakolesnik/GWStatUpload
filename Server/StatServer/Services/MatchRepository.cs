using StatServer.Contexts;
using StatServer.Entities;

namespace StatServer.Services
{
	internal class MatchRepository
	{
		private readonly AppDbContext _ctx;

		public MatchRepository(AppDbContext ctx)
		{
			_ctx = ctx;
		}

		public IEnumerable<Match> GetAllMatches()
		{
			return _ctx.Matches;
		}

		public IEnumerable<MatchEntry> GetMatchEntriesForMatch(int matchId)
		{
			return _ctx.MatchEntries.Where(x => x.MatchId == matchId);
		}

		public bool AddMatchEntry(MatchEntry entry)
		{
			// Worry later about reconciling multiple submissions of the same match

			_ctx.MatchEntries.Add(entry);
			int rowsAdded = _ctx.SaveChanges();
			return rowsAdded > 0;
		}
	}
}
