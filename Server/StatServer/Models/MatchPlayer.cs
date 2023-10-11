namespace StatServer.Models
{
	public class MatchPlayer
	{
		public string Name { get; set; } = string.Empty;
		public HashSet<MatchPlayerSkill> Skills { get; set; } = new();

		public MatchPlayer(string name)
		{
			Name = name;
		}
	}
}
