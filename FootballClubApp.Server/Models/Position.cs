using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballClubApp.Server.Models
{
    public class Position
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PositionId { get; set; }

        [Required, MaxLength(100), Display(Name = "Position Name")]
        public string PositionName { get; set; }

        [Required, MaxLength(500), Display(Name = "Description")]
        public string Description { get; set; }

        public List<PlayerDetail>? PlayerDetails { get; set; }
    }
} // Position Model (GK, ST, etc.)
