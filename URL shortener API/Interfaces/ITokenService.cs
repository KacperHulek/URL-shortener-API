using URL_shortener_API.Models;

namespace URL_shortener_API.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
