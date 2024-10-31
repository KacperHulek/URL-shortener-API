using URL_shortener_API.Dtos;

namespace URL_shortener_API.Interfaces
{
    public interface IUrlRepository
    {
        Task<UrlShortenResponseDto> ShortenUrlAsync(UrlShortenRequestDto request);
    }
}
