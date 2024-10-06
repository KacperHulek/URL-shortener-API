using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URL_shortener_API.Dtos
{
    public class UrlShortenRequestDto
    {
        public string OriginalUrl { get; set; }
    }

    public class UrlShortenResponseDto
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
    }
}
