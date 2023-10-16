using StatServer.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatServer.Entities
{
	[Table("Matches")]
	public class Match
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public DateTime Submitted { get; set; }
		public int InstanceId { get; set; }
		public Team WinningTeam { get; set; } = Team.Unknown; //TODO - figure out how to populate this

		// nav
		public List<MatchEntry>? MatchEntries { get; set; }

		public Match()
		{
			Submitted = DateTime.UtcNow;
			WinningTeam = Team.Unknown;
		}

		public Match(Team result) : this()
		{
			WinningTeam = result;
		}
	}
}
