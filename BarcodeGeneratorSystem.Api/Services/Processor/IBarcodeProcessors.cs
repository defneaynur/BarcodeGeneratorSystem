using BarcodeGeneratorSystem.Domain.Models.DatabaseModel;
using BarcodeGeneratorSystem.Domain.Models.RequestModel;
using BarcodeGeneratorSystem.Domain.Models.ResponseModel;
using Dapper;
using Moonlight.ExceptionHandling.Exceptions;
using System.Data;

namespace BarcodeGeneratorSystem.Api.Services.Processor
{
    public interface IBarcodeProcessors
    {
        Task<BarcodeGenerateResponse> GenerateBarcodeAsync();
        Task<Barcodes> CreateBarcodeAsync(Barcodes barcode);
        Task<IEnumerable<Barcodes>> GetBarcodesAsync();
        Task<Barcodes> GetBarcodeByIdAsync(BarcodeIdRequest barcode);
        Task<BarcodeValidateResponse> ValidateBarcodeAsync(BarcodeRequest barcode);
    }
    public class BarcodeProcessors(IDbConnection _dbConnection) : IBarcodeProcessors
    {
        /// <summary>
        /// Generate barcode method
        /// </summary>
        /// <returns></returns>
        public async Task<BarcodeGenerateResponse> GenerateBarcodeAsync()
        {
            string gtin11, gtin12;
            var random = new Random();


            gtin11 = "";
            for (int i = 0; i < 11; i++)
                gtin11 += random.Next(0, 10);

            gtin12 = gtin11 + CalculateCheckDigit(gtin11);

            var barcode = new BarcodeGenerateResponse
            {
                Gtin12 = gtin12,
                Created = DateTime.Now
            };

            return barcode;
        }

        /// <summary>
        /// Create barcode method
        /// </summary>
        /// <param name="barcode">Gtin12 format</param>
        /// <returns></returns>
        public async Task<Barcodes> CreateBarcodeAsync(Barcodes barcode)
        {
            const string query = @"
                INSERT INTO Barcodes (Gtin12, Creator, Created, IsDeleted)
                VALUES (@Gtin12, @Creator, @Created, @IsDeleted)";

            var result = await _dbConnection.ExecuteAsync(query, barcode);

            return barcode;
        }

        /// <summary>
        /// List All Barcode
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Barcodes>> GetBarcodesAsync()
        {
            const string query = "SELECT * FROM Barcodes WHERE IsDeleted = 0 OR IsDeleted IS NULL ORDER BY Created DESC";

            var result = await _dbConnection.QueryAsync<Barcodes>(query);

            return result;
        }

        /// <summary>
        /// List barcode by id
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public async Task<Barcodes> GetBarcodeByIdAsync(BarcodeIdRequest barcode)
        {
            const string query = "SELECT * FROM Barcodes WHERE Id = @Id AND (IsDeleted IS NULL OR IsDeleted = 0)";

            var result = await _dbConnection.QuerySingleOrDefaultAsync<Barcodes>(query, new { Id = barcode.Id });
            if (result == null)
                throw new CoreException("Record Not Found");

            return result;
        }

        /// <summary>
        /// Validate barcode to GS1
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public async Task<BarcodeValidateResponse> ValidateBarcodeAsync(BarcodeRequest barcode)
        {
            if (barcode == null || string.IsNullOrWhiteSpace(barcode.Gtin12))
            {
                return new BarcodeValidateResponse { IsValid = false };
            }

            return new BarcodeValidateResponse { IsValid = ValidateBarcode(barcode.Gtin12) };
        }

        #region Private Methods
        /// <summary>
        /// Calculate end code number for gtin11
        /// </summary>
        /// <param name="gtin11">gtin11</param>
        /// <returns></returns>
        private string CalculateCheckDigit(string gtin11)
        {
            int sum = 0;
            for (int i = 0; i < 11; i++)
            {
                int digit = gtin11[i] - '0';
                sum += (i % 2 == 0) ? digit * 3 : digit;
            }

            int check = (10 - (sum % 10)) % 10;
            return check.ToString();
        }

        /// <summary>
        /// Validate gtin12 
        /// </summary>
        /// <param name="gtin">gtin12</param>
        /// <returns></returns>
        private bool ValidateBarcode(string gtin)
        {
            var gtinData = gtin.Trim();

            if (gtin.Length != 12 || !gtinData.All(char.IsDigit))
                return false;

            string gtin11 = gtin.Substring(0, 11);
            char actualCheckDigit = gtin[11];

            int sum = 0;
            for (int i = 0; i < 11; i++)
            {
                int digit = gtin11[i] - '0';
                sum += (i % 2 == 0) ? digit * 3 : digit;
            }

            int expectedCheck = (10 - (sum % 10)) % 10;
            char expectedCheckDigit = expectedCheck.ToString()[0];

            bool isValid = actualCheckDigit == expectedCheckDigit;
            return isValid;
        }
        #endregion
    }
}
