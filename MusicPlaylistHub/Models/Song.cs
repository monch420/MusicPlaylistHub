using System.ComponentModel.DataAnnotations;

namespace MusicPlaylistHub.Models
{
    public class Song
    {
        [Key] 
        public int id { get; set; } 

        public string title { get; set; }

        public string artist { get; set; }

        public string album { get; set; }

        public string duration { get; set; }
    }
}
