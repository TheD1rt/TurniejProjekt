namespace Tournament.Models;

public class Match
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public int BracketId { get; set; }
    public int? Player1Id { get; set; }
    public int? Player2Id { get; set; }
    public int? WinnerId { get; set; }
    public string Status { get; set; } = "pending";
    public int? Player1Score { get; set; }
    public int? Player2Score { get; set; }
    public DateTime ScheduledAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    
    public Tournament? Tournament { get; set; }
    public Bracket? Bracket { get; set; }
    public User? Player1 { get; set; }
    public User? Player2 { get; set; }
    public User? Winner { get; set; }
}
