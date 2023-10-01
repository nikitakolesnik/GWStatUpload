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
		public Team Result { get; set; }

		// nav
 public List<MatchEntry>? MatchEntries { get; set; }

 public Match()
		{
			Submitted = DateTime.Now;
			Result = Team.Unknown;
		}

		public Match(Team result) : this()
		{
			Result = result;
		}
	}
}
