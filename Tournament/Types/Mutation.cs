using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Tournament.Data.Context;
using Tournament.Models;
using Tournament.Services;

namespace Tournament.Types;

public class Mutation
{
    public async Task<UserType> Register(
        [Service] TournamentContext context,
        string username,
        string email,
        string password,
        string firstName,
        string lastName)
    {
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == email);
        if (existingUser != null)
            throw new InvalidOperationException("User already exists");

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            FirstName = firstName,
            LastName = lastName,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

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

    public async Task<string> Login(
        [Service] TournamentContext context,
        [Service] JwtService jwtService,
        string username,
        string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new InvalidOperationException("Invalid credentials");

        return jwtService.GenerateToken(user);
    }

    public async Task<TournamentType> CreateTournament(
        [Service] TournamentContext context,
        [Service] TournamentService tournamentService,
        string name,
        string description,
        int creatorId,
        int maxParticipants)
    {
        var tournament = await tournamentService.CreateTournamentAsync(name, description, creatorId, maxParticipants);

        return new TournamentType
        {
            Id = tournament.Id,
            Name = tournament.Name,
            Description = tournament.Description,
            StartDate = tournament.StartDate,
            CreatorId = tournament.CreatorId,
            Status = tournament.Status,
            MaxParticipants = tournament.MaxParticipants,
            BracketType = tournament.BracketType
        };
    }
}
