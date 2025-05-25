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
    public class BarcodeService(IBarcodeProcessors _barcodeProcessors) : ControllerBase
    {

        [Authorize]
        [HttpPost("codes")]
        public async Task<CoreResponse<Barcodes>> GenerateBarcodeAsync()
        {
            var result = await _barcodeProcessors.GenerateBarcodeAsync();

            if (string.IsNullOrEmpty(result.Gtin12))
            {
                return new CoreResponse<Barcodes>
                {
                    Data = null,
                    CoreResponseCode = CoreResponseCode.NoData,
                    ErrorMessages = new List<string>(),
                    Message = "Barcode üretilemedi."
                };
            }

            var data = new Barcodes
            {
                Gtin12 = result.Gtin12,
                Creator = User?.Identity?.Name,
                Created = result.Created,
                IsDeleted = false,
            };

            var createData = await _barcodeProcessors.CreateBarcodeAsync(data);
            return new CoreResponse<Barcodes>
            {
                Data = data,
                CoreResponseCode = CoreResponseCode.Success,
                ErrorMessages = new List<string>(),
                Message = ""
            };
        }

        [HttpGet("codes")]
        public async Task<CoreResponse<IEnumerable<Barcodes>>> GetBarcodesAsync()
        {
            var result = await _barcodeProcessors.GetBarcodesAsync();
            if (!result.Any())
            {
                return new CoreResponse<IEnumerable<Barcodes>>
                {
                    Data = null,
                    CoreResponseCode = CoreResponseCode.NoData,
                    ErrorMessages = new List<string>(),
                    Message = "Aradığınız kriterlerde data bulunamadı."
                };
            }

            return new CoreResponse<IEnumerable<Barcodes>>
            {
                Data = result,
                CoreResponseCode = CoreResponseCode.Success,
                ErrorMessages = new List<string>(),
                Message = ""
            };
        }

        [HttpGet("codes/{id}")]
        public async Task<CoreResponse<Barcodes>> GetBarcodeByIdAsync(long id)
        {
            var result = await _barcodeProcessors.GetBarcodeByIdAsync(new BarcodeIdRequest { Id = id });

            return new CoreResponse<Barcodes>
            {
                Data = result,
                CoreResponseCode = CoreResponseCode.Success,
                ErrorMessages = new List<string>(),
                Message = ""
            };
        }


        [Authorize]
        [HttpPost("validate")]
        public async Task<CoreResponse<BarcodeValidateResponse>> ValidateBarcode([FromBody] BarcodeRequest barcodeRequest)
        {
            var result = _barcodeProcessors.ValidateBarcodeAsync(barcodeRequest);
            return new CoreResponse<BarcodeValidateResponse>
            {
                Data = result.Result,
                CoreResponseCode = CoreResponseCode.Success,
                ErrorMessages = new List<string>(),
            };

        }



    }
}
