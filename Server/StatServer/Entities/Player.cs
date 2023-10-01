using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatServer.Entities
{
	[Table("Players")]
	public class Player
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Name { get; set; }

 public Player(string name)
 {
			Name = name;
 }
 }
}
