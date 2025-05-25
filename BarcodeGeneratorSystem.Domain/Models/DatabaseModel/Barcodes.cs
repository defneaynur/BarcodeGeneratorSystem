using BarcodeGeneratorSystem.Domain.Models.Base;

namespace BarcodeGeneratorSystem.Domain.Models.DatabaseModel
{
    public class Barcodes : BaseModel
    {
        public long Id { get; set; }
        public string Gtin12 { get; set; }
    }
}
