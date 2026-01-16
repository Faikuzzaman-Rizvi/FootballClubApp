namespace FootballClubApp.Server.DTOs
{
    public class ClubInputDto
    {
        public string ClubCode { get; set; }
        public string ClubName { get; set; }
        public DateTime FoundedDate { get; set; }
        public string StadiumName { get; set; }
        public bool IsActive { get; set; }
        public List<PlayerInputDto>? Players { get; set; }
    }

    public class PlayerInputDto
    {
        public string PlayerCode { get; set; }
        public string PlayerName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public int JerseyNumber { get; set; }
        public List<PlayerDetailInputDto>? PlayerDetails { get; set; }
    }

    public class PlayerDetailInputDto
    {
        public string PositionName { get; set; }
        public DateTime ContractStart { get; set; }
        public DateTime ContractEnd { get; set; }
        public decimal AnnualSalary { get; set; }
        public int GoalsScored { get; set; }
        public int Assists { get; set; }
        public int MatchesPlayed { get; set; }
    }
}