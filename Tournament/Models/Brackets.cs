namespace Tournament.Models;

public class Bracket
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Round { get; set; }
    public string Status { get; set; } = "pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    
    public Tournament? Tournament { get; set; }
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}
