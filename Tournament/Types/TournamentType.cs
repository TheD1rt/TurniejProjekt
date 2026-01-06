using Tournament.Models;

namespace Tournament.Types;

public class TournamentType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int CreatorId { get; set; }
    public string Status { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }
    public string BracketType { get; set; } = string.Empty;

    public UserType? Creator { get; set; }
    public ICollection<BracketType> Brackets { get; set; } = new List<BracketType>();
    public ICollection<MatchType> Matches { get; set; } = new List<MatchType>();
}
