using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace v2.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        [Display(Name = "Player Name")]
        public string PlayerName { get; set; } = "";

//////////////////////////////////

        public Team? Team { get; set; }
    }

}
