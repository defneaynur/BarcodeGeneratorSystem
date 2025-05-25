using BarcodeGeneratorSystem.Api.Services;
using BarcodeGeneratorSystem.Api.Services.Processor;
using BarcodeGeneratorSystem.Domain.Models.DatabaseModel;
using BarcodeGeneratorSystem.Domain.Models.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moonlight.Response.Response;
using Moq;
using System.Security.Claims;

public class ProductServiceTests
{
    private readonly Mock<IErpProcessors> _mockErpProcessors = new();
    private readonly Mock<IProductProcessors> _mockProductProcessors = new();

    private ProductService CreateControllerWithUser()
    {
        var controller = new ProductService(_mockErpProcessors.Object, _mockProductProcessors.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "deneme")
        }, "mock"));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        return controller;
    }

    [Fact]
    public async Task ImportFromErpService_ShouldReturnSuccess_WhenProductsImported()
    {
        // Arrange
        var erpProducts = new List<ErpProductResponse>
        {
            new() { Id = 1, Name = "Ürün 1", Sku = "123" },
            new() { Id = 2, Name = "Ürün 2", Sku = "456" },
        };

        _mockErpProcessors.Setup(x => x.GetErpProductsAsync()).ReturnsAsync(new CoreResponse<IEnumerable<ErpProductResponse>>
        {
            Data = erpProducts,
            CoreResponseCode = CoreResponseCode.Success
        });

        _mockProductProcessors.Setup(x => x.ImportProductsAsync(It.IsAny<IEnumerable<Product>>())).ReturnsAsync(2);

        var controller = CreateControllerWithUser();

        // Act
        var result = await controller.ImportFromErpService();

        // Assert
        Assert.Equal(CoreResponseCode.Success, result.CoreResponseCode);
        Assert.Equal("ERP verileri başarıyla kaydedildi.Count= 2", result.Message);
        Assert.Equal(erpProducts, result.Data);
    }

    [Fact]
    public async Task ImportFromErpService_ShouldReturnNoData_WhenErpDataIsEmpty()
    {
        _mockErpProcessors.Setup(x => x.GetErpProductsAsync()).ReturnsAsync(new CoreResponse<IEnumerable<ErpProductResponse>>
        {
            Data = [],
            CoreResponseCode = CoreResponseCode.Success
        });

        var controller = CreateControllerWithUser();

        var result = await controller.ImportFromErpService();

        Assert.Equal(CoreResponseCode.NoData, result.CoreResponseCode);
        Assert.Equal("Erp verisi alınamadı.", result.Message);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task ImportFromErpService_ShouldReturnSuccess_WhenNoNewProductsToImport()
    {
        var erpProducts = new List<ErpProductResponse>
        {
            new() { Id = 1, Name = "Ürün 1", Sku = "123" },
        };

        _mockErpProcessors.Setup(x => x.GetErpProductsAsync()).ReturnsAsync(new CoreResponse<IEnumerable<ErpProductResponse>>
        {
            Data = erpProducts,
            CoreResponseCode = CoreResponseCode.Success
        });

        _mockProductProcessors.Setup(x => x.ImportProductsAsync(It.IsAny<IEnumerable<Product>>())).ReturnsAsync(0);

        var controller = CreateControllerWithUser();

        var result = await controller.ImportFromErpService();

        Assert.Equal(CoreResponseCode.Success, result.CoreResponseCode);
        Assert.Equal("Aktarılacak yeni veri yok ", result.Message);
    }
}
