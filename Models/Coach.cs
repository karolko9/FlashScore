using System.ComponentModel.DataAnnotations;

namespace v2.Models
{
    public class Coach
    {
        [Key]
        public int CoachId { get; set; }

        [Display(Name = "Coach Name")]
        public string CoachName { get; set; } = "";



///////////////////////////////////////
        public ICollection<Team>? Teams { get; set; }

    }

}
