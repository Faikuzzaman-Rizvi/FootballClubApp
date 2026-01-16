using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballClubApp.Server.Models
{
    public class PlayerDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerDetailId { get; set; }

        [ForeignKey(nameof(Player))]
        public int PlayerId { get; set; }

        [ForeignKey(nameof(Position))]
        public int PositionId { get; set; }

        [Required, Display(Name = "Contract Start")]
        [DataType(DataType.Date)]
        public DateTime ContractStart { get; set; }

        [Required, Display(Name = "Contract End")]
        [DataType(DataType.Date)]
        public DateTime ContractEnd { get; set; }

        [Required, Display(Name = "Annual Salary")]
        [DataType(DataType.Currency)]
        public decimal AnnualSalary { get; set; }

        [Display(Name = "Goals Scored")]
        public int GoalsScored { get; set; }

        [Display(Name = "Assists")]
        public int Assists { get; set; }

        [Display(Name = "Matches Played")]
        public int MatchesPlayed { get; set; }

        public Player? Player { get; set; }
        public Position? Position { get; set; }
    }
}// PlayerDetail Model (Stats, Contract Info)
