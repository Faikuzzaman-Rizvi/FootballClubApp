using FootballClubApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballClubApp.Server.Data
{
    public class FootballClubDbContext : DbContext
    {
        public FootballClubDbContext(DbContextOptions<FootballClubDbContext> options)
            : base(options)
        {
        }

        public DbSet<Club> Clubs { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<PlayerDetail> PlayerDetails { get; set; } = null!;
        public DbSet<Position> Positions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Club → Player (One to Many)
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Club)
                .WithMany(c => c.Players)
                .HasForeignKey(p => p.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            // Player → PlayerDetail (One to Many)
            modelBuilder.Entity<PlayerDetail>()
                .HasKey(pd => pd.PlayerDetailId);

            modelBuilder.Entity<PlayerDetail>()
                .HasOne(pd => pd.Player)
                .WithMany(p => p.PlayerDetails)
                .HasForeignKey(pd => pd.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerDetail>()
                .HasOne(pd => pd.Position)
                .WithMany(pos => pos.PlayerDetails)
                .HasForeignKey(pd => pd.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data - Positions
            modelBuilder.Entity<Position>().HasData(
                new Position { PositionId = 1, PositionName = "Goalkeeper (GK)", Description = "Defends the goal" },
                new Position { PositionId = 2, PositionName = "Defender (CB)", Description = "Central defender" },
                new Position { PositionId = 3, PositionName = "Left Back (LB)", Description = "Left side defender" },
                new Position { PositionId = 4, PositionName = "Right Back (RB)", Description = "Right side defender" },
                new Position { PositionId = 5, PositionName = "Midfielder (CM)", Description = "Central midfielder" },
                new Position { PositionId = 6, PositionName = "Attacking Midfielder (CAM)", Description = "Creative playmaker" },
                new Position { PositionId = 7, PositionName = "Winger (LW/RW)", Description = "Side attacker" },
                new Position { PositionId = 8, PositionName = "Striker (ST)", Description = "Forward, main goal scorer" }
            );

            // Seed data - Clubs
            modelBuilder.Entity<Club>().HasData(
                new Club
                {
                    ClubId = 1,
                    ClubCode = "MU-2025",
                    ClubName = "Manchester United",
                    FoundedDate = new DateTime(1878, 1, 1),
                    StadiumName = "Old Trafford",
                    IsActive = true,
                    ClubLogo = "/images/clubs/manchester-united.png"
                },
                new Club
                {
                    ClubId = 2,
                    ClubCode = "FCB-2025",
                    ClubName = "FC Barcelona",
                    FoundedDate = new DateTime(1899, 11, 29),
                    StadiumName = "Camp Nou",
                    IsActive = true,
                    ClubLogo = "/images/clubs/barcelona.png"
                },
                new Club
                {
                    ClubId = 3,
                    ClubCode = "RM-2025",
                    ClubName = "Real Madrid",
                    FoundedDate = new DateTime(1902, 3, 6),
                    StadiumName = "Santiago Bernabéu",
                    IsActive = true,
                    ClubLogo = "/images/clubs/real-madrid.png"
                }
            );

            // Seed data - Players
            modelBuilder.Entity<Player>().HasData(
                new Player
                {
                    PlayerId = 1,
                    PlayerCode = "CR7-2025",
                    PlayerName = "Cristiano Ronaldo",
                    DateOfBirth = new DateTime(1985, 2, 5),
                    Nationality = "Portugal",
                    JerseyNumber = 7,
                    IsActive = true,
                    PlayerPhoto = "/images/players/ronaldo.jpg",
                    ClubId = 1
                },
                new Player
                {
                    PlayerId = 2,
                    PlayerCode = "LM10-2025",
                    PlayerName = "Lionel Messi",
                    DateOfBirth = new DateTime(1987, 6, 24),
                    Nationality = "Argentina",
                    JerseyNumber = 10,
                    IsActive = true,
                    PlayerPhoto = "/images/players/messi.jpg",
                    ClubId = 2 
                },
                new Player
                {
                    PlayerId = 3,
                    PlayerCode = "NJR11-2025",
                    PlayerName = "Neymar Jr",
                    DateOfBirth = new DateTime(1992, 2, 5),
                    Nationality = "Brazil",
                    JerseyNumber = 11,
                    IsActive = true,
                    PlayerPhoto = "/images/players/neymar.jpg",
                    ClubId = 2 
                }
            );

            // Seed data - PlayerDetails
            modelBuilder.Entity<PlayerDetail>().HasData(         
                new PlayerDetail
                {
                    PlayerDetailId = 1,
                    PlayerId = 1,
                    PositionId = 8,
                    ContractStart = new DateTime(2024, 1, 1),
                    ContractEnd = new DateTime(2026, 12, 31),
                    AnnualSalary = 50000000M,
                    GoalsScored = 145,
                    Assists = 45,
                    MatchesPlayed = 200
                },

                new PlayerDetail
                {
                    PlayerDetailId = 2,
                    PlayerId = 2,
                    PositionId = 6,
                    ContractStart = new DateTime(2023, 7, 1),
                    ContractEnd = new DateTime(2025, 6, 30),
                    AnnualSalary = 55000000M,
                    GoalsScored = 672,
                    Assists = 305,
                    MatchesPlayed = 778
                },

                new PlayerDetail
                {
                    PlayerDetailId = 3,
                    PlayerId = 3,
                    PositionId = 7,
                    ContractStart = new DateTime(2024, 1, 1),
                    ContractEnd = new DateTime(2027, 12, 31),
                    AnnualSalary = 45000000M,
                    GoalsScored = 436,
                    Assists = 237,
                    MatchesPlayed = 649
                }
            );
        }
    }
}
