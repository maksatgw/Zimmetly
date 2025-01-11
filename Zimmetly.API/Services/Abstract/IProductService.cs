using Zimmetly.API.Models;

namespace Zimmetly.API.Services.Abstract
{
    public interface IProductService
    {
        IQueryable<Product> Get(string? searchQuery = null);
        IQueryable<Product> FindOne(List<int>? idList);
        Task UpdateAsync(Product product);
        Task InsertAsync(Product product);
        Task DeleteAsync(List<Product> products);
    }
}
