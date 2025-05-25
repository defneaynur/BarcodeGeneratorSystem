using BarcodeGeneratorSystem.Api.Services;
using BarcodeGeneratorSystem.Api.Services.Processor;
using BarcodeGeneratorSystem.Domain.Models.DatabaseModel;
using BarcodeGeneratorSystem.Domain.Models.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moonlight.Response.Response;
using Moq;
using System.Security.Claims;

public class BarcodeServiceTests
{
    private readonly Mock<IBarcodeProcessors> _mockBarcodeProcessors;
    private readonly BarcodeService _barcodeService;

    public BarcodeServiceTests()
    {
        _mockBarcodeProcessors = new Mock<IBarcodeProcessors>();
        _barcodeService = new BarcodeService(_mockBarcodeProcessors.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "TestUser")
        }, "mock"));

        _barcodeService.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };
    }

    [Fact]
    public async Task GenerateBarcodeAsync_ReturnsSuccess_WhenBarcodeGenerated()
    {
        // Arrange
        var barcodeGenerateResponse = new BarcodeGenerateResponse
        {
            Gtin12 = "123456789012",
            Created = DateTime.Now
        };

        var expectedBarcode = new Barcodes
        {
            Gtin12 = barcodeGenerateResponse.Gtin12,
            Created = barcodeGenerateResponse.Created,
            Creator = "TestUser",
            IsDeleted = false
        };

        _mockBarcodeProcessors
            .Setup(x => x.GenerateBarcodeAsync())
            .ReturnsAsync(barcodeGenerateResponse);

        _mockBarcodeProcessors
            .Setup(x => x.CreateBarcodeAsync(It.IsAny<Barcodes>()))
            .ReturnsAsync(expectedBarcode);

        // Act
        var response = await _barcodeService.GenerateBarcodeAsync();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(CoreResponseCode.Success, response.CoreResponseCode);
        Assert.NotNull(response.Data);
        Assert.Equal(expectedBarcode.Gtin12, response.Data.Gtin12);
        Assert.Equal("TestUser", response.Data.Creator);
    }
}
