using Newtonsoft.Json;

namespace StatServer.Models
{
	public class MatchEntryDTO
	{
		public DateTime Submitted { get; set; } = DateTime.Now;
 public List<MatchPlayer> Players { get; set; } = new();
	}

	public class MatchPlayer
	{
		public string Name { get; set; } = string.Empty;
		public List<MatchPlayerSkill> Skills { get; set; } = new();
	}

	public class MatchPlayerSkill
	{
		[JsonProperty("name")]
		public string Name { get; set; } = string.Empty;

		[JsonProperty("skill_id")]
		public int Id { get; set; }

		[JsonProperty("count")]
		public int Count { get; set; }
	}
}
