using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballClubApp.Server.Models
{
    public class Player 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerId { get; set; }

        [Required, StringLength(20), Display(Name = "Player Code")]
        public string PlayerCode { get; set; }

        [Required, MaxLength(100), Display(Name = "Player Name")]
        public string PlayerName { get; set; }

        [Required, DataType(DataType.Date), Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required, MaxLength(50), Display(Name = "Nationality")]
        public string Nationality { get; set; }

        [Required, Display(Name = "Jersey Number")]
        public int JerseyNumber { get; set; }

        [Required, Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Required, DataType(DataType.ImageUrl), Display(Name = "Player Photo")]
        public string PlayerPhoto { get; set; }

        // Foreign Key to Club
        [ForeignKey(nameof(Club))]
        public int ClubId { get; set; }
        public Club? Club { get; set; }

        public List<PlayerDetail>? PlayerDetails { get; set; }
    }
}
// Player Model (Belongs to Club)