
using Microsoft.AspNetCore.Mvc;
using URL_shortener_API.Dtos;
using URL_shortener_API.Models;
using URL_shortener_API.Helpers;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Cors;
using URL_shortener_API.Data;

namespace URL_shortener_API.Controllers
{
    [Route("api/UrlShortener")]
    [ApiController]
    public class UrlShortenerController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly string _baseUrl = "https://localhost:7143/";

        public UrlShortenerController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost("shortenUrl")]
        public async Task<IActionResult> ShortenUrl([FromBody] UrlShortenRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.OriginalUrl))
            {
                return BadRequest("The URL cannot be empty.");
            }

            if (!System.Uri.IsWellFormedUriString(request.OriginalUrl, UriKind.RelativeOrAbsolute))
            {
                return BadRequest("Invalid URL format. Please provide a valid URL.");
            }

            var existingUrl = _context.ShortUrls.FirstOrDefault(u =>
                u.OriginalUrl == request.OriginalUrl
            );

            if (existingUrl != null)
            {
                return Ok(
                    new UrlShortenResponseDto
                    {
                        Id = existingUrl.Id,
                        ShortCode = existingUrl.ShortCode,
                        OriginalUrl = existingUrl.OriginalUrl,
                    }
                );
            }

            var shortCode = CodeGenerator.GenerateShortCode();

            var shortUrl = new ShortUrl
            {
                OriginalUrl = request.OriginalUrl,
                ShortCode = shortCode,
            };

            _context.ShortUrls.Add(shortUrl);
            await _context.SaveChangesAsync();

            return Ok(
                new UrlShortenResponseDto
                {
                    ShortCode = shortCode,
                    OriginalUrl = shortUrl.OriginalUrl,
                    Id = shortUrl.Id,
                }
            );
        }

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

        [HttpGet("getAllUrls")]
        public IActionResult GetAll()
        {
            var urls = _context.ShortUrls.Select(u => new UrlShortenResponseDto
            {
                Id = u.Id,
                OriginalUrl = u.OriginalUrl,
                ShortCode = u.ShortCode
            }).ToList();
            return Ok(urls);
        }
    }
}
