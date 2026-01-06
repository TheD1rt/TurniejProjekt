using Microsoft.EntityFrameworkCore;
using Tournament.Models;

namespace Tournament.Data.Context;

public class TournamentContext : DbContext
{
    public TournamentContext(DbContextOptions<TournamentContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Models.Tournament> Tournaments { get; set; } = null!;
    public DbSet<Bracket> Brackets { get; set; } = null!;
    public DbSet<Match> Matches { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfiguracja User
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<User>()
            .HasMany(u => u.TournamentsCreated)
            .WithOne(t => t.Creator)
            .HasForeignKey(t => t.CreatorId);

        // Konfiguracja Tournament
        modelBuilder.Entity<Models.Tournament>()
            .HasKey(t => t.Id);
        modelBuilder.Entity<Models.Tournament>()
            .HasMany(t => t.Brackets)
            .WithOne(b => b.Tournament)
            .HasForeignKey(b => b.TournamentId);
        modelBuilder.Entity<Models.Tournament>()
            .HasMany(t => t.Matches)
            .WithOne(m => m.Tournament)
            .HasForeignKey(m => m.TournamentId);

        // Konfiguracja Bracket
        modelBuilder.Entity<Bracket>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<Bracket>()
            .HasMany(b => b.Matches)
            .WithOne(m => m.Bracket)
            .HasForeignKey(m => m.BracketId);

        // Konfiguracja Match
        modelBuilder.Entity<Match>()
            .HasKey(m => m.Id);
        modelBuilder.Entity<Match>()
            .HasOne(m => m.Player1)
            .WithMany(u => u.MatchesPlayed)
            .HasForeignKey(m => m.Player1Id)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Match>()
            .HasOne(m => m.Player2)
            .WithMany()
            .HasForeignKey(m => m.Player2Id)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Match>()
            .HasOne(m => m.Winner)
            .WithMany()
            .HasForeignKey(m => m.WinnerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
