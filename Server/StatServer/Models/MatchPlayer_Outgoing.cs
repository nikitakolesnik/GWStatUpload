namespace StatServer.Models
{
	public class MatchPlayer_Outgoing
	{
		public string Name { get; set; } = string.Empty;
		public string Team { get; set; }
		public HashSet<MatchPlayerSkill> Skills { get; set; } = new();

		public MatchPlayer_Outgoing(string name, string team)
		{
			Name = name;
			Team = team;
		}
	}
}
