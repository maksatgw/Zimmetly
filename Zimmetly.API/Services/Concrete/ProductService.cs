using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Zimmetly.API.Context;
using Zimmetly.API.Models;
using Zimmetly.API.Services.Abstract;

namespace Zimmetly.API.Services.Concrete
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAsync()
        {
            try
            {
                var products = await _context.Products.Include(products=>products.Attachments).ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in GetAsync: {ex.Message}");
                throw new Exception("An error occurred while retrieving the products.", ex);
            }
        }

        public IQueryable<Product> Get(string? searchQuery = null)
        {
            try
            {
                IQueryable<Product> query = _context.Products.Include(products => products.Attachments);

                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    query = query.Where(p => p.Name.Contains(searchQuery) 
                    || p.Serial.Contains(searchQuery)
                    || p.Description.Contains(searchQuery));
                }

                return query;
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama
                Console.Error.WriteLine($"Error in Get: {ex.Message}");
                throw new Exception("An error occurred while searching for products.", ex);
            }
        }


        public IQueryable<Product> FindOne(List<int>? idList)
        {
            if (idList == null || !idList.Any())
            {
                var query = _context.Products;
                return query;
            }
            try
            {
                var query = _context.Products
                    .Where(p => idList.Contains(p.Id));
                return query;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in Find: {ex.Message}");
                throw new Exception("An error occurred while finding the products.", ex);
            }
        }
        public async Task DeleteAsync(List<Product> products)
        {
            if (products == null || !products.Any())
            {
                throw new ArgumentException("The product list cannot be null or empty.", nameof(products));
            }

            try
            {
                _context.Products.RemoveRange(products);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while deleting the products from the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the products.", ex);
            }
        }


        public async Task InsertAsync(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "The product cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("The product name cannot be null or empty.", nameof(product.Name));
            }
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving the product to the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while inserting the product.", ex);
            }
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        
    }
}
