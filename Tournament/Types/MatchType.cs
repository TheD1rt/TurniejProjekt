namespace Tournament.Types;

public class MatchType
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public int BracketId { get; set; }
    public int? Player1Id { get; set; }
    public int? Player2Id { get; set; }
    public int? WinnerId { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? Player1Score { get; set; }
    public int? Player2Score { get; set; }
    public DateTime ScheduledAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public TournamentType? Tournament { get; set; }
    public BracketType? Bracket { get; set; }
    public UserType? Player1 { get; set; }
    public UserType? Player2 { get; set; }
    public UserType? Winner { get; set; }
}
