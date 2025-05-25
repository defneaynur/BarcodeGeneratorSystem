using Core.Config.Config.Model;

namespace Core.Config.Config
{
    public class ApiInformations
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public JwtSettings JwtSettings { get; set; }
        public List<AllowedOrigins> AllowedOrigins { get; set; }
    }
}
