using Tournament.Models;
using Tournament.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Tournament.Services;

public class TournamentService
{
    private readonly TournamentContext _context;

    public TournamentService(TournamentContext context)
    {
        _context = context;
    }

    
    public async Task<Models.Tournament> CreateTournamentAsync(string name, string description, int creatorId, int maxParticipants)
    {
        var tournament = new Models.Tournament
        {
            Name = name,
            Description = description,
            CreatorId = creatorId,
            MaxParticipants = maxParticipants,
            StartDate = DateTime.UtcNow,
            Status = "pending",
            BracketType = "single_elimination"
        };

        _context.Tournaments.Add(tournament);
        await _context.SaveChangesAsync();

        return tournament;
    }

    
    public async Task<List<Bracket>> GenerateBracketsAsync(int tournamentId)
    {
        var tournament = await _context.Tournaments.FindAsync(tournamentId);
        if (tournament == null)
            throw new ArgumentException("Turniej nie znaleziony");

        var brackets = new List<Bracket>();
        int round = 1;

        
        int participants = tournament.MaxParticipants;
        while (participants > 1)
        {
            var bracket = new Bracket
            {
                TournamentId = tournamentId,
                Name = $"Round {round}",
                Round = round,
                Status = "pending"
            };

            _context.Brackets.Add(bracket);
            brackets.Add(bracket);

            participants = (participants + 1) / 2;
            round++;
        }

        await _context.SaveChangesAsync();
        return brackets;
    }

    
    public async Task<List<Match>> GenerateMatchesForBracketAsync(int bracketId, List<int> playerIds)
    {
        var bracket = await _context.Brackets
            .Include(b => b.Matches)
            .FirstOrDefaultAsync(b => b.Id == bracketId);

        if (bracket == null)
            throw new ArgumentException("Bracket nie znaleziony");

        var matches = new List<Match>();
        int tournamentId = bracket.TournamentId;

        
        for (int i = 0; i < playerIds.Count; i += 2)
        {
            var match = new Match
            {
                TournamentId = tournamentId,
                BracketId = bracketId,
                Player1Id = playerIds[i],
                Player2Id = i + 1 < playerIds.Count ? playerIds[i + 1] : null,
                Status = "pending",
                ScheduledAt = DateTime.UtcNow.AddDays(1)
            };

            _context.Matches.Add(match);
            matches.Add(match);
        }

        await _context.SaveChangesAsync();
        return matches;
    }

    
    public async Task<Match> UpdateMatchResultAsync(int matchId, int winnerId, int? player1Score = null, int? player2Score = null)
    {
        var match = await _context.Matches.FindAsync(matchId);
        if (match == null)
            throw new ArgumentException("Mecz nie znaleziony");

        match.WinnerId = winnerId;
        match.Player1Score = player1Score;
        match.Player2Score = player2Score;
        match.Status = "completed";
        match.CompletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return match;
    }

   
    public async Task<List<Match>> GenerateNextRoundAsync(int tournamentId, int currentBracketId)
    {
        var currentBracket = await _context.Brackets
            .Include(b => b.Matches)
            .FirstOrDefaultAsync(b => b.Id == currentBracketId);

        if (currentBracket == null)
            throw new ArgumentException("Bracket nie znaleziony");

        
        var winners = currentBracket.Matches
            .Where(m => m.WinnerId.HasValue)
            .Select(m => m.WinnerId.Value)
            .ToList();

        var unpairedWinners = currentBracket.Matches
            .Where(m => m.Player2Id == null && m.WinnerId.HasValue)
            .Select(m => m.WinnerId.Value)
            .ToList();

        if (unpairedWinners.Count > 0)
        {
            winners.AddRange(unpairedWinners);
        }

        
        var nextBracket = new Bracket
        {
            TournamentId = tournamentId,
            Name = $"Round {currentBracket.Round + 1}",
            Round = currentBracket.Round + 1,
            Status = "pending"
        };

        _context.Brackets.Add(nextBracket);
        await _context.SaveChangesAsync();

        
        return await GenerateMatchesForBracketAsync(nextBracket.Id, winners);
    }

    
    public async Task<Models.Tournament?> GetTournamentAsync(int tournamentId)
    {
        return await _context.Tournaments
            .Include(t => t.Creator)
            .Include(t => t.Brackets)
            .Include(t => t.Matches)
            .FirstOrDefaultAsync(t => t.Id == tournamentId);
    }

    
    public async Task<List<Models.Tournament>> GetAllTournamentsAsync()
    {
        return await _context.Tournaments
            .Include(t => t.Creator)
            .ToListAsync();
    }
}
