using Newtonsoft.Json;

namespace StatServer.Models
{
	public class MatchPlayerSkill
	{
		[JsonProperty("skill_id")]
		public int Id { get; set; }

		[JsonProperty("count")]
		public int Count { get; set; }

		public MatchPlayerSkill(int id, int count)
		{
			Id = id;
			Count = count;
		}
	}
}
