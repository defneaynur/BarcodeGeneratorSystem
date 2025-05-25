using BarcodeGeneratorSystem.Domain.Models.DatabaseModel;
using Dapper;
using System.Data;

namespace BarcodeGeneratorSystem.Api.Services.Processor
{
    public interface IProductProcessors
    {
        Task<int> ImportProductsAsync(IEnumerable<Product> products);
    }
    public class ProductProcessors(IDbConnection _dbConnection) : IProductProcessors
    {


        public async Task<int> ImportProductsAsync(IEnumerable<Product> products)
        {
            var existingSkus = await _dbConnection.QueryAsync<string>("SELECT Sku FROM Product WHERE Sku IN @Skus",
                                           new { Skus = products.Select(p => p.Sku).ToArray() });

            var productsToInsert = products.Where(p => !existingSkus.Contains(p.Sku)).ToList();

            if (!productsToInsert.Any())
                return 0;

            const string query = @"
                    INSERT INTO Product (Name, Sku, Creator, Created, IsDeleted)
                    VALUES (@Name, @Sku, @Creator, @Created, @IsDeleted)";

            var result = await _dbConnection.ExecuteAsync(query, productsToInsert);

            return result;
        }

    }
}
