using Newtonsoft.Json;

namespace StatServer.Models
{
	public class MatchEntryDTO
	{
		public DateTime Submitted { get; set; } = DateTime.Now;
		public HashSet<MatchPlayer> Players { get; set; } = new();

        public MatchEntryDTO(DateTime submitted)
        {
			Submitted = submitted;
        }
    }

	public class MatchPlayer
	{
		public string Name { get; set; } = string.Empty;
		public HashSet<MatchPlayerSkill> Skills { get; set; } = new();

        public MatchPlayer(string name)
        {
			Name = name;
        }
    }

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
