namespace URL_shortener_API.Helpers
{
    public class CodeGenerator
    {
        public static string GenerateShortCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(chars, 7).Select(s => s[random.Next(s.Length)]).ToArray()
            );
        }
    }


}

