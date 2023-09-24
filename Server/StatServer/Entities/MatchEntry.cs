using StatServer.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatServer.Entities
{
	[Table("MatchEntries")]
	internal class MatchEntry
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MatchId { get; set; }
		public Team Team { get; set; } = Team.Unknown;
        public int PlayerId { get; set; }
		public int SkillId { get; set; }
		public int Count { get; set; }

		//nav
		public Match Match { get; set; }
		public Player Player { get; set; }
	}
}
