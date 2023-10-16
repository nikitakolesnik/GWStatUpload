using Newtonsoft.Json;

namespace StatServer.Models
{
	public class MatchEntryDTO_Incoming
	{
		[JsonProperty("instance_id")]
		public UInt32 InstanceId { get; set; }
		[JsonProperty("team_id")]
		public byte TeamId { get; set; } // equivalent of uint8_t?

		public HashSet<MatchPlayer> Players { get; set; } = new();
	}
}
