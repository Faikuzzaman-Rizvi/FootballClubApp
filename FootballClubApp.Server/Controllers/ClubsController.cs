using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballClubApp.Server.Data;
using FootballClubApp.Server.Models;
using System.Text.Json;
using FootballClubApp.Server.DTOs;

namespace FootballClubApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly FootballClubDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ClubsController(FootballClubDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Helper method to delete file from wwwroot
        private void DeleteFileIfExists(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) &&
                !filePath.Contains("default.png") &&
                !filePath.Contains("default.jpg"))
            {
                var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    try
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting file {fullPath}: {ex.Message}");
                    }
                }
            }
        }

        // GET: api/Clubs
        [HttpGet]
        public async Task<IActionResult> GetAllClubs()
        {
            var clubs = await _context.Clubs
                .Include(c => c.Players)
                    .ThenInclude(p => p.PlayerDetails)
                        .ThenInclude(pd => pd.Position)
                .OrderByDescending(c => c.ClubId)
                .ToListAsync();
            return Ok(clubs);
        }

        // GET: api/Clubs/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetClubById(int id)
        {
            var club = await _context.Clubs
                .Include(c => c.Players)
                    .ThenInclude(p => p.PlayerDetails)
                        .ThenInclude(pd => pd.Position)
                .FirstOrDefaultAsync(c => c.ClubId == id);

            if (club == null)
            {
                return NotFound();
            }
            return Ok(club);
        }

        // POST: api/Clubs
        [HttpPost]
        public async Task<IActionResult> AddClub([FromForm] string clubData,
            [FromForm] IFormFile? clubLogo,
            [FromForm] IFormFileCollection? playerPhotos,
            [FromForm] string? photoIndices)
        {
            try
            {
                var input = JsonSerializer.Deserialize<ClubInputDto>(clubData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (input == null)
                {
                    return BadRequest("Invalid club data");
                }

                List<int> indices = new List<int>();
                if (!string.IsNullOrEmpty(photoIndices))
                {
                    indices = JsonSerializer.Deserialize<List<int>>(photoIndices) ?? new List<int>();
                }
                // Create club
                var club = new Club
                {
                    ClubCode = input.ClubCode,
                    ClubName = input.ClubName,
                    FoundedDate = input.FoundedDate,
                    StadiumName = input.StadiumName,
                    IsActive = input.IsActive,
                    ClubLogo = "/images/clubs/default.png"
                };

                // Handle club logo upload
                if (clubLogo != null && clubLogo.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "clubs");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(clubLogo.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await clubLogo.CopyToAsync(stream);
                    }
                    club.ClubLogo = "/images/clubs/" + uniqueFileName;
                }

                _context.Clubs.Add(club);
                await _context.SaveChangesAsync();

                // Add players
                if (input.Players != null && input.Players.Any())
                {
                    int photoFileIndex = 0;
                    for (int i = 0; i < input.Players.Count; i++)
                    {
                        var playerInput = input.Players[i];
                        var player = new Player
                        {
                            PlayerCode = playerInput.PlayerCode,
                            PlayerName = playerInput.PlayerName,
                            DateOfBirth = playerInput.DateOfBirth,
                            Nationality = playerInput.Nationality,
                            JerseyNumber = playerInput.JerseyNumber,
                            IsActive = true,
                            PlayerPhoto = "/images/players/default.jpg",
                            ClubId = club.ClubId
                        };

                        // Handle player photo upload
                        if (indices.Contains(i) && playerPhotos != null && photoFileIndex < playerPhotos.Count)
                        {
                            var playerPhoto = playerPhotos[photoFileIndex];
                            if (playerPhoto != null && playerPhoto.Length > 0)
                            {
                                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "players");
                                Directory.CreateDirectory(uploadsFolder);

                                var uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(playerPhoto.FileName);
                                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await playerPhoto.CopyToAsync(stream);
                                }
                                player.PlayerPhoto = "/images/players/" + uniqueFileName;
                            }
                            photoFileIndex++;
                        }

                        _context.Players.Add(player);
                        await _context.SaveChangesAsync();

                        // Add player details
                        if (playerInput.PlayerDetails != null && playerInput.PlayerDetails.Any())
                        {
                            foreach (var detailInput in playerInput.PlayerDetails)
                            {
                                var position = await _context.Positions
                                    .FirstOrDefaultAsync(p => p.PositionName == detailInput.PositionName);

                                if (position != null)
                                {
                                    var playerDetail = new PlayerDetail
                                    {
                                        PlayerId = player.PlayerId,
                                        PositionId = position.PositionId,
                                        ContractStart = detailInput.ContractStart,
                                        ContractEnd = detailInput.ContractEnd,
                                        AnnualSalary = detailInput.AnnualSalary,
                                        GoalsScored = detailInput.GoalsScored,
                                        Assists = detailInput.Assists,
                                        MatchesPlayed = detailInput.MatchesPlayed
                                    };
                                    _context.PlayerDetails.Add(playerDetail);
                                }
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                var result = await _context.Clubs
                    .Include(c => c.Players)
                        .ThenInclude(p => p.PlayerDetails)
                            .ThenInclude(pd => pd.Position)
                    .FirstOrDefaultAsync(c => c.ClubId == club.ClubId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        // PUT: api/Clubs/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateClub(int id,
            [FromForm] string clubData,
            [FromForm] IFormFile? clubLogo,
            [FromForm] IFormFileCollection? playerPhotos,
            [FromForm] string? photoIndices)
        {
            try
            {
                var input = JsonSerializer.Deserialize<ClubInputDto>(clubData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (input == null)
                {
                    return BadRequest("Invalid club data");
                }

                // Parse photo indices
                List<int> indices = new List<int>();
                if (!string.IsNullOrEmpty(photoIndices))
                {
                    indices = JsonSerializer.Deserialize<List<int>>(photoIndices) ?? new List<int>();
                }

                var existingClub = await _context.Clubs
                    .Include(c => c.Players)
                        .ThenInclude(p => p.PlayerDetails)
                    .FirstOrDefaultAsync(c => c.ClubId == id);

                if (existingClub == null)
                {
                    return NotFound();
                }

                // Update club properties
                existingClub.ClubCode = input.ClubCode;
                existingClub.ClubName = input.ClubName;
                existingClub.FoundedDate = input.FoundedDate;
                existingClub.StadiumName = input.StadiumName;
                existingClub.IsActive = input.IsActive;

                // Handle club logo upload
                if (clubLogo != null && clubLogo.Length > 0)
                {                 
                    DeleteFileIfExists(existingClub.ClubLogo);

                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "clubs");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(clubLogo.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await clubLogo.CopyToAsync(stream);
                    }
                    existingClub.ClubLogo = "/images/clubs/" + uniqueFileName;
                }
                
                var existingPlayerPhotos = existingClub.Players?
                    .Select((p, index) => new { Index = index, Photo = p.PlayerPhoto })
                    .ToDictionary(x => x.Index, x => x.Photo) ?? new Dictionary<int, string>();

                // Delete existing players and their details
                if (existingClub.Players != null && existingClub.Players.Any())
                {
                    foreach (var player in existingClub.Players)
                    {                      
                    }
                    _context.Players.RemoveRange(existingClub.Players);
                }
                await _context.SaveChangesAsync();

                // Add new/updated players
                if (input.Players != null && input.Players.Any())
                {
                    int photoFileIndex = 0;
                    for (int i = 0; i < input.Players.Count; i++)
                    {
                        var playerInput = input.Players[i];
                        var player = new Player
                        {
                            PlayerCode = playerInput.PlayerCode,
                            PlayerName = playerInput.PlayerName,
                            DateOfBirth = playerInput.DateOfBirth,
                            Nationality = playerInput.Nationality,
                            JerseyNumber = playerInput.JerseyNumber,
                            IsActive = true,
                            ClubId = existingClub.ClubId
                        };

                        // Determine player photo
                        bool hasNewPhoto = indices.Contains(i) && playerPhotos != null && photoFileIndex < playerPhotos.Count;

                        if (hasNewPhoto)
                        {
                            // New photo uploaded
                            if (existingPlayerPhotos.ContainsKey(i))
                            {
                                DeleteFileIfExists(existingPlayerPhotos[i]);
                            }

                            var playerPhoto = playerPhotos[photoFileIndex];
                            if (playerPhoto != null && playerPhoto.Length > 0)
                            {
                                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "players");
                                Directory.CreateDirectory(uploadsFolder);

                                var uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(playerPhoto.FileName);
                                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await playerPhoto.CopyToAsync(stream);
                                }
                                player.PlayerPhoto = "/images/players/" + uniqueFileName;
                            }
                            else
                            {
                                player.PlayerPhoto = "/images/players/default.jpg";
                            }
                            photoFileIndex++;
                        }
                        else
                        {
                            if (existingPlayerPhotos.ContainsKey(i) && !string.IsNullOrEmpty(existingPlayerPhotos[i]))
                            {
                                player.PlayerPhoto = existingPlayerPhotos[i];
                            }
                            else
                            {
                                player.PlayerPhoto = "/images/players/default.jpg";
                            }
                        }

                        _context.Players.Add(player);
                        await _context.SaveChangesAsync();

                        // Add player details
                        if (playerInput.PlayerDetails != null && playerInput.PlayerDetails.Any())
                        {
                            foreach (var detailInput in playerInput.PlayerDetails)
                            {
                                var position = await _context.Positions
                                    .FirstOrDefaultAsync(p => p.PositionName == detailInput.PositionName);

                                if (position != null)
                                {
                                    var playerDetail = new PlayerDetail
                                    {
                                        PlayerId = player.PlayerId,
                                        PositionId = position.PositionId,
                                        ContractStart = detailInput.ContractStart,
                                        ContractEnd = detailInput.ContractEnd,
                                        AnnualSalary = detailInput.AnnualSalary,
                                        GoalsScored = detailInput.GoalsScored,
                                        Assists = detailInput.Assists,
                                        MatchesPlayed = detailInput.MatchesPlayed
                                    };
                                    _context.PlayerDetails.Add(playerDetail);
                                }
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                foreach (var kvp in existingPlayerPhotos)
                {
                    if (kvp.Key >= (input.Players?.Count ?? 0) ||
                        (indices.Contains(kvp.Key) && playerPhotos != null))
                    {
                        DeleteFileIfExists(kvp.Value);
                    }
                }

                var result = await _context.Clubs
                    .Include(c => c.Players)
                        .ThenInclude(p => p.PlayerDetails)
                            .ThenInclude(pd => pd.Position)
                    .FirstOrDefaultAsync(c => c.ClubId == existingClub.ClubId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        // DELETE: api/Clubs/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var existingClub = await _context.Clubs
                .Include(c => c.Players)
                    .ThenInclude(p => p.PlayerDetails)
                .FirstOrDefaultAsync(c => c.ClubId == id);

            if (existingClub == null)
            {
                return NotFound();
            }
            DeleteFileIfExists(existingClub.ClubLogo);

            if (existingClub.Players != null && existingClub.Players.Any())
            {
                foreach (var player in existingClub.Players)
                {
                    DeleteFileIfExists(player.PlayerPhoto);
                }
            }

            _context.Clubs.Remove(existingClub);
            await _context.SaveChangesAsync();
            return Ok(existingClub);
        }

        // GET: api/Clubs/GetPositions
        [HttpGet("GetPositions")]
        public async Task<IActionResult> GetAllPositions()
        {
            var positions = await _context.Positions.ToListAsync();
            return Ok(positions);
        }
    }
}