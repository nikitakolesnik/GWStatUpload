using Newtonsoft.Json;

namespace StatServer.Models
{
	public class MatchEntryDTO_Incoming
	{
		[JsonProperty("instance_id")]
		public UInt32 InstanceId { get; set; }
		[JsonProperty("team_id")]
		public byte TeamId { get; set; }

		public HashSet<MatchPlayer> Players { get; set; } = new();
	}
}
