namespace StatServer.Models
{
	public class MatchPlayer_Incoming
	{
		public string Name { get; set; } = string.Empty;
		public HashSet<MatchPlayerSkill> Skills { get; set; } = new();

		public MatchPlayer_Incoming(string name)
		{
			Name = name;
		}
	}
}
