using BarcodeGeneratorSystem.Domain.Models.Base;

namespace BarcodeGeneratorSystem.Domain.Models.DatabaseModel
{
    public class Product : BaseModel
    {
        public int Id { get; set; }  // DB'de Identity
        public string Name { get; set; }
        public string Sku { get; set; }
    }

}
