namespace StatServer.Models
{
	public class MatchEntry_Outgoing
	{
		public DateTime Submitted { get; set; }
		public HashSet<MatchPlayer_Outgoing> Players { get; set; } = new();
		
		public MatchEntry_Outgoing(DateTime submitted)
		{
			Submitted = submitted;
		}
	}
}
