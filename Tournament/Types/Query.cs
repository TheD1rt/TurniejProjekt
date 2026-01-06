using Tournament.Data.Context;
using Tournament.Services;
using Microsoft.EntityFrameworkCore;

namespace Tournament.Types;

public class Query
{
    public async Task<TournamentType?> GetTournament(
        [Service] TournamentContext context,
        int id)
    {
        var tournament = await context.Tournaments
            .Include(t => t.Creator)
            .Include(t => t.Brackets)
            .Include(t => t.Matches)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tournament == null)
            return null;

        return new TournamentType
        {
            Id = tournament.Id,
            Name = tournament.Name,
            Description = tournament.Description,
            StartDate = tournament.StartDate,
            EndDate = tournament.EndDate,
            CreatorId = tournament.CreatorId,
            Status = tournament.Status,
            MaxParticipants = tournament.MaxParticipants,
            BracketType = tournament.BracketType,
            Creator = tournament.Creator != null ? new UserType
            {
                Id = tournament.Creator.Id,
                Username = tournament.Creator.Username,
                Email = tournament.Creator.Email,
                FirstName = tournament.Creator.FirstName,
                LastName = tournament.Creator.LastName,
                CreatedAt = tournament.Creator.CreatedAt,
                IsActive = tournament.Creator.IsActive
            } : null
        };
    }

    public async Task<List<TournamentType>> GetAllTournaments(
        [Service] TournamentContext context)
    {
        var tournaments = await context.Tournaments
            .Include(t => t.Creator)
            .ToListAsync();

        return tournaments.Select(t => new TournamentType
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            StartDate = t.StartDate,
            EndDate = t.EndDate,
            CreatorId = t.CreatorId,
            Status = t.Status,
            MaxParticipants = t.MaxParticipants,
            BracketType = t.BracketType,
            Creator = t.Creator != null ? new UserType
            {
                Id = t.Creator.Id,
                Username = t.Creator.Username,
                Email = t.Creator.Email,
                FirstName = t.Creator.FirstName,
                LastName = t.Creator.LastName,
                CreatedAt = t.Creator.CreatedAt,
                IsActive = t.Creator.IsActive
            } : null
        }).ToList();
    }

    public async Task<UserType?> GetUser(
        [Service] TournamentContext context,
        int id)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return null;

        return new UserType
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive
        };
    }
}
