namespace Tournament.Models;

public class Tournament
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int CreatorId { get; set; }
    public string Status { get; set; } = "pending";
    public int MaxParticipants { get; set; }
    public string BracketType { get; set; } = "single_elimination";

    
    public User? Creator { get; set; }
    public ICollection<Bracket> Brackets { get; set; } = new List<Bracket>();
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}
