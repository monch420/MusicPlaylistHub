using MusicPlaylistHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;


namespace MusicPlaylistHub.Controllers
{
    public class MusicController : Controller
    {
        private readonly MusicDbContext _context;

        public MusicController(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            var songs = await _context.Songs.ToListAsync();
            return View(songs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Song song)
        {
            if (ModelState.IsValid)
            {
                var totalSongs = await _context.Songs.CountAsync();
                song.id = totalSongs + 1;  
                _context.Songs.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(song);
        }

        public async Task<ActionResult> Details(string title)
        {
            var song = await _context.Songs.FirstOrDefaultAsync(s => s.title == title);
            if (song == null)
            {
                return NotFound();
            }
            return View(song);
        }

        public IActionResult SearchLyrics()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchLyrics(string artist, string title)
        {
            if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(title))
            {
                ViewBag.Error = "Both artist and title must be provided.";
                return View();
            }

            string apiUrl = $"https://api.lyrics.ovh/v1/{artist}/{title}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(apiUrl);
                    dynamic lyricsResult = JsonConvert.DeserializeObject(response);
                    ViewBag.Lyrics = lyricsResult.lyrics;
                }
                catch (HttpRequestException)
                {
                    ViewBag.Error = "Lyrics not found. Please check the artist and title.";
                }
            }

            return View();
        }

        public IActionResult Remove()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string artist, string title)
        {
            if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(title))
            {
                ViewBag.Error = "Both artist and title must be provided.";
                return View();
            }

            var song = await _context.Songs.FirstOrDefaultAsync(s => s.artist == artist && s.title == title);
            if (song == null)
            {
                ViewBag.Error = "Song not found. Please check the artist and title.";
                return View();
            }

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
