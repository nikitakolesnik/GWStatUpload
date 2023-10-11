namespace StatServer.Models
{
	public class MatchEntryDTO_Outgoing
	{
		public DateTime Submitted { get; set; }
		public HashSet<MatchPlayer> Players { get; set; } = new();
		
		public MatchEntryDTO_Outgoing(DateTime submitted)
		{
			Submitted = submitted;
		}
	}
}
