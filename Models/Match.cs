using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v2.Models
{
    public class Match
    {
        [Key]
        public int MatchId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Match Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime MatchDate { get; set; }

        [Display(Name = "Result Home Team")]
        public int? ResultHomeTeam { get; set; }

        [Display(Name = "Result Away Team")]
        public int? ResultAwayTeam { get; set; }

        // public int? HomeTeamId { get; set; } 
        public Team? HomeTeam { get; set; }

        // public int? AwayTeamId { get; set; }
        public Team? AwayTeam { get; set; }
        // public ICollection<Match>? Matches { get; set; }

    }

}

