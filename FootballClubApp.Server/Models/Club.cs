using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace FootballClubApp.Server.Models
{
    public class Club
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClubId { get; set; }

        [Required, StringLength(20), Display(Name = "Club Code")]
        public string ClubCode { get; set; }

        [Required, MaxLength(100), Display(Name = "Club Name")]
        public string ClubName { get; set; }

        [Required, DataType(DataType.Date), Display(Name = "Founded Date")]
        public DateTime FoundedDate { get; set; }

        [Required, MaxLength(100), Display(Name = "Stadium Name")]
        public string StadiumName { get; set; }

        [Required, Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Required, DataType(DataType.ImageUrl), Display(Name = "Club Logo")]
        public string ClubLogo { get; set; }

        public List<Player>? Players { get; set; }
    }
} // Club Model (Main Entity)
