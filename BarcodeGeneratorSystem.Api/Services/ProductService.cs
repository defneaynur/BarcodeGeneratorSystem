using BarcodeGeneratorSystem.Api.Services.Processor;
using BarcodeGeneratorSystem.Api.Services.Secure;
using BarcodeGeneratorSystem.Domain.Models.DatabaseModel;
using BarcodeGeneratorSystem.Domain.Models.RequestModel;
using BarcodeGeneratorSystem.Domain.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Moonlight.Response.Response;
using System.ComponentModel.Design;

namespace BarcodeGeneratorSystem.Api.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductService(IErpProcessors _erpProcessors, IProductProcessors _productProcessors) : ControllerBase
    {

        [HttpGet("products/external")]
        public async Task<CoreResponse<IEnumerable<ErpProductResponse>>> ImportFromErpService()
        {
            var erpProducts = await _erpProcessors.GetErpProductsAsync();

            if (erpProducts.Data.Count() == 0)
            {
                return new CoreResponse<IEnumerable<ErpProductResponse>>
                {
                    Data = null,
                    CoreResponseCode = CoreResponseCode.NoData,
                    ErrorMessages = new List<string>(),
                    Message = "Erp verisi alınamadı."
                };
            }

            var data = erpProducts.Data.Select(p => new Product
            {
                Name = p.Name,
                Sku = p.Sku,
                Creator = User?.Identity?.Name ?? "system",
                Created = DateTime.Now
            });


            var createData = await _productProcessors.ImportProductsAsync(data);
            if (createData == 0)
            {
                return new CoreResponse<IEnumerable<ErpProductResponse>>
                {
                    Data = erpProducts.Data,
                    CoreResponseCode = CoreResponseCode.Success,
                    ErrorMessages = new List<string>(),
                    Message = "Aktarılacak yeni veri bulunamadı."
                };
            }
            return new CoreResponse<IEnumerable<ErpProductResponse>>
            {
                Data = erpProducts.Data,
                CoreResponseCode = CoreResponseCode.Success,
                ErrorMessages = new List<string>(),
                Message = "ERP verileri başarıyla kaydedildi. Count= " + createData
            };
        }
    }
}
