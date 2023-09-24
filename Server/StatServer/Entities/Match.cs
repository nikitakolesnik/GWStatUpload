using StatServer.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatServer.Entities
{
	[Table("Matches")]
	internal class Match
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public DateTime Submitted { get; set; }
		public Team Result { get; set; }

		public Match()
		{
			Submitted = DateTime.Now;
		}

		public Match(Team result) : this()
		{
			Result = result;
		}
	}
}
