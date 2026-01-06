namespace Tournament.Types;

public class BracketType
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Round { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public TournamentType? Tournament { get; set; }
    public ICollection<MatchType> Matches { get; set; } = new List<MatchType>();
}
