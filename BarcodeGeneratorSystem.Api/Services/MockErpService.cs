using BarcodeGeneratorSystem.Api.Services.Processor;
using BarcodeGeneratorSystem.Domain.Models.DatabaseModel;
using BarcodeGeneratorSystem.Domain.Models.RequestModel;
using BarcodeGeneratorSystem.Domain.Models.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlight.Response.Response;

namespace BarcodeGeneratorSystem.Api.Services
{
    [ApiController]
    [Route("api/[controller]")]
    public class MockErpService() : ControllerBase
    {
        [HttpGet("erpMockData")]
        public async Task<CoreResponse<IEnumerable<ErpProductResponse>>> GetProducts()
        {
            return new CoreResponse<IEnumerable<ErpProductResponse>>
            {
                Data = ProductData(),
                CoreResponseCode = CoreResponseCode.Success,
                ErrorMessages = new List<string>(),
            };
        }

        #region Private Methods
        private IEnumerable<ErpProductResponse> ProductData()
        {
            var products = new List<ErpProductResponse>
                {
                    new ErpProductResponse { Id = 1, Name = "Ürün 1", Sku = "123456789012" },
                    new ErpProductResponse { Id = 2, Name = "Ürün 2", Sku = "234567890123" },
                    new ErpProductResponse { Id = 3, Name = "Ürün 3", Sku = "345678901234" },
                    new ErpProductResponse { Id = 4, Name = "Ürün 4", Sku = "456789012345" },
                    new ErpProductResponse { Id = 5, Name = "Ürün 5", Sku = "656789012345" },
                    new ErpProductResponse { Id = 6, Name = "Ürün 1", Sku = "123" }

                };
            return products;
        }
        #endregion
    }
}
