using Microsoft.EntityFrameworkCore;

namespace URL_shortener_API.Models;

public class UrlShortenerContext : DbContext
{
    public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options)
        : base(options) { }

    public DbSet<ShortUrl> ShortUrls { get; set; } = null!;
}
