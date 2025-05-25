using BarcodeGeneratorSystem.Domain.Models.ResponseModel;
using Moonlight.Response.Response;

namespace BarcodeGeneratorSystem.Api.Services.Processor
{
    public interface IErpProcessors
    {
        Task<CoreResponse<IEnumerable<ErpProductResponse>>> GetErpProductsAsync();
    }

    public class ErpProcessors(HttpClient _httpClient, ILogger<ErpProcessors> _logger) : IErpProcessors
    {
        /// <summary>
        /// Call Mock Erp Service for mock product data
        /// </summary>
        /// <returns></returns>
        public async Task<CoreResponse<IEnumerable<ErpProductResponse>>> GetErpProductsAsync()
        {
            int retryCount = 0;
            while (retryCount < 3)
            {
                try
                {
                    var result = await _httpClient.GetFromJsonAsync<CoreResponse<IEnumerable<ErpProductResponse>>>("api/MockErpService/erpMockData");

                    if (result != null)
                        return result;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"ERP verisi alınamadı. Deneme: {retryCount + 1}, Hata: {ex.Message}");
                    await Task.Delay(1000);
                    retryCount++;
                }
            }

            _logger.LogError("ERP verisi alınamadı, fallback çalıştı.");
            return new CoreResponse<IEnumerable<ErpProductResponse>>(); 
        }
    }

}
