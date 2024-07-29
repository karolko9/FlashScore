using System.ComponentModel.DataAnnotations;

namespace v2.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Team Name")]
        public string TeamName { get; set; } = "";

        [Display(Name = "Wins")]
        public int Wins { get; set; } = 0;

//////////////////////////////////////
        public Coach? Coach { get; set; }

///////////////////////////////////////////
        
        public ICollection<Player>? Players { get; set; }
        // public ICollection<Match>? Matches { get; set; }



    }

}
