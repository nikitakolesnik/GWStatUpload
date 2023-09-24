using System.Text.Json.Serialization;

namespace StatServer.DTOs
{
	public class MatchEntryDTO
	{
		public List<PlayerDTO> Players { get; set; }
	}

	public class PlayerDTO
	{
		public List<SkillDTO> Skills { get; set; }
	}

	public class SkillDTO
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("skill_id")]
		public int SkillId { get; set; }

		[JsonPropertyName("count")]
		public int Count { get; set; }
	}
}
