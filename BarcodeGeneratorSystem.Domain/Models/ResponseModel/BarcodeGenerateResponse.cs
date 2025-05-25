namespace BarcodeGeneratorSystem.Domain.Models.ResponseModel
{
    public class BarcodeGenerateResponse
    {
        public string Gtin12 { get; set; }
        public string ImageBarcode { get; set; }
        public DateTime Created { get; set; }
    }
}
