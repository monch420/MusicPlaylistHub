using Microsoft.EntityFrameworkCore;

namespace MusicPlaylistHub.Models
{
    public class MusicDbContext : DbContext
    {
        public MusicDbContext(DbContextOptions<MusicDbContext> options)
            : base(options)
        {
        }

        public DbSet<Song> Songs { get; set; }
    }
}
