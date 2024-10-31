using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.CodeDom.Compiler;
using URL_shortener_API.Data;
using URL_shortener_API.Dtos;
using URL_shortener_API.Helpers;
using URL_shortener_API.Interfaces;
using URL_shortener_API.Models;

namespace URL_shortener_API.Repository
{
    public class UrlRepository : IUrlRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly Helpers.CodeGenerator _codeGenerator;
        public UrlRepository(ApplicationDBContext context, Helpers.CodeGenerator codeGenerator)
        {
            _context = context;
            _codeGenerator = codeGenerator;
        }

        public async Task<UrlShortenResponseDto> ShortenUrlAsync(UrlShortenRequestDto request)
        {
            var existingUrl = _context.ShortUrls.FirstOrDefault(u =>
            u.OriginalUrl == request.OriginalUrl
            );

            if (existingUrl != null)
            {
                return (
                    new UrlShortenResponseDto
                    {
                        Id = existingUrl.Id,
                        ShortCode = existingUrl.ShortCode,
                        OriginalUrl = existingUrl.OriginalUrl,
                    }
                );
            }

            var shortCode = _codeGenerator.GenerateShortCode();

            var shortUrl = new ShortUrl
            {
                OriginalUrl = request.OriginalUrl,
                ShortCode = shortCode,
            };

            _context.ShortUrls.Add(shortUrl);
            await _context.SaveChangesAsync();

            return (
                new UrlShortenResponseDto
                {
                    ShortCode = shortCode,
                    OriginalUrl = shortUrl.OriginalUrl,
                    Id = shortUrl.Id,
                }
            );
        }
    }
}
