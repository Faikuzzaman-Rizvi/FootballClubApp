using FootballClubApp.Server.Data;
using FootballClubApp.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballClubApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly FootballClubDbContext _context;

        public PlayersController(FootballClubDbContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<IActionResult> GetAllPlayers()
        {
            var players = await _context.Players
                .Include(p => p.Club)
                .Include(p => p.PlayerDetails)
                    .ThenInclude(pd => pd.Position)
                .OrderByDescending(p => p.PlayerId)
                .ToListAsync();
            return Ok(players);
        }

        // GET: api/Players/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPlayerById(int id)
        {
            var player = await _context.Players
                .Include(p => p.Club)
                .Include(p => p.PlayerDetails)
                    .ThenInclude(pd => pd.Position)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }

        // POST: api/Players
        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromForm] Player player, [FromForm] int[] positionIds, [FromForm] IFormFile playerPhoto)
        {
            // Handle player photo upload
            if (playerPhoto != null && playerPhoto.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/players");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + "_" + playerPhoto.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await playerPhoto.CopyToAsync(stream);
                }
                player.PlayerPhoto = "/images/players/" + uniqueFileName;
            }

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            // Add player details (positions)
            if (positionIds != null && positionIds.Length > 0)
            {
                foreach (var positionId in positionIds)
                {
                    var playerDetail = new PlayerDetail
                    {
                        PlayerId = player.PlayerId,
                        PositionId = positionId,
                        ContractStart = DateTime.Now,
                        ContractEnd = DateTime.Now.AddYears(2),
                        AnnualSalary = 0,
                        GoalsScored = 0,
                        Assists = 0,
                        MatchesPlayed = 0
                    };
                    _context.PlayerDetails.Add(playerDetail);
                }
                await _context.SaveChangesAsync();
            }

            return Ok(player);
        }

        // PUT: api/Players/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePlayer(int id, [FromForm] Player player, [FromForm] int[] positionIds, [FromForm] IFormFile playerPhoto)
        {
            var existingPlayer = await _context.Players
                .Include(p => p.PlayerDetails)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (existingPlayer == null)
            {
                return NotFound();
            }

            // Handle player photo upload
            if (playerPhoto != null && playerPhoto.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/players");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + "_" + playerPhoto.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await playerPhoto.CopyToAsync(stream);
                }
                existingPlayer.PlayerPhoto = "/images/players/" + uniqueFileName;
            }

            // Update player properties
            existingPlayer.PlayerCode = player.PlayerCode;
            existingPlayer.PlayerName = player.PlayerName;
            existingPlayer.DateOfBirth = player.DateOfBirth;
            existingPlayer.Nationality = player.Nationality;
            existingPlayer.JerseyNumber = player.JerseyNumber;
            existingPlayer.IsActive = player.IsActive;
            existingPlayer.ClubId = player.ClubId;

            // Update player details (positions)
            _context.PlayerDetails.RemoveRange(existingPlayer.PlayerDetails);

            if (positionIds != null && positionIds.Length > 0)
            {
                foreach (var positionId in positionIds)
                {
                    var playerDetail = new PlayerDetail
                    {
                        PlayerId = existingPlayer.PlayerId,
                        PositionId = positionId,
                        ContractStart = DateTime.Now,
                        ContractEnd = DateTime.Now.AddYears(2),
                        AnnualSalary = 0,
                        GoalsScored = 0,
                        Assists = 0,
                        MatchesPlayed = 0
                    };
                    _context.PlayerDetails.Add(playerDetail);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(existingPlayer);
        }

        // DELETE: api/Players/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var existingPlayer = await _context.Players
                .Include(p => p.PlayerDetails)
                .FirstOrDefaultAsync(p => p.PlayerId == id);

            if (existingPlayer == null)
            {
                return NotFound();
            }

            _context.Players.Remove(existingPlayer);
            await _context.SaveChangesAsync();
            return Ok(existingPlayer);
        }
    }
}
