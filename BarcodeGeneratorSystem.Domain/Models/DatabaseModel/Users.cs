using BarcodeGeneratorSystem.Domain.Models.Base;

namespace BarcodeGeneratorSystem.Domain.Models.DatabaseModel
{
    public class Users : BaseModel
    {
        public long Id { get; set; }
        public string? UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
