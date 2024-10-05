using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using URL_shortener_API.Models;

namespace URL_shortener_API.Controllers
{
    [Route("api/UrlShortener")]
    [ApiController]
    public class UrlShortenerController : Controller
    {
        private readonly UrlShortenerContext _context;

        public UrlShortenerController(UrlShortenerContext context)
        {
            _context = context;
        }

        [HttpPost("shortenUrl")]
        public async Task<IActionResult> ShortenUrl([FromBody] string originalUrl)
        {
            if (string.IsNullOrWhiteSpace(originalUrl))
            {
                return BadRequest("The URL cannot be empty.");
            }

            var existingUrl = _context.ShortUrls.FirstOrDefault(u => u.OriginalUrl == originalUrl);
            if (existingUrl != null)
            {
                return Ok(new { shortUrl = $"https://localhost:5001/{existingUrl.ShortCode}" });
            }

            var shortCode = GenerateShortCode();

            var shortUrl = new ShortUrl { OriginalUrl = originalUrl, ShortCode = shortCode };

            _context.ShortUrls.Add(shortUrl);
            await _context.SaveChangesAsync();

            return Ok(new { shortUrl = $"https://localhost:5001/{shortCode}" });
        }

        private string GenerateShortCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray()
            );
        }

        // Get: /{shortCode}
        [HttpGet("{shortCode}")]
        public IActionResult RedirectToOriginalUrl(string shortCode)
        {
            // Find the URL in the database using the short code
            var shortUrl = _context.ShortUrls.FirstOrDefault(s => s.ShortCode == shortCode);

            if (shortUrl == null)
            {
                return NotFound("URL not found.");
            }

            // Redirect to the original URL
            return Redirect(shortUrl.OriginalUrl);
        }
    }
}
