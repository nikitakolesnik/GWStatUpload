using Newtonsoft.Json;

namespace StatServer.Models
{
	public class MatchEntry_Incoming
	{
		[JsonProperty("instance_id")]
		public UInt32 InstanceId { get; set; }
		[JsonProperty("team_id")]
		public byte TeamId { get; set; } // byte = uint8_t ?

		public HashSet<MatchPlayer_Incoming> Players { get; set; } = new();
	}
}
