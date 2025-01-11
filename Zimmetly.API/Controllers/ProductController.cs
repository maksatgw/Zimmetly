using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zimmetly.API.Context;
using Zimmetly.API.DTOs;
using Zimmetly.API.Models;
using Zimmetly.API.Services.Abstract;
using Zimmetly.API.Services.Concrete;
using Zimmetly.API.ViewModels;

namespace Zimmetly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRenderPdfService _renderPdfService;
        private readonly IProductService _productService;
        private readonly IAttachmentService _attachmentService;

        public ProductController(AppDbContext context, IProductService productService, IAttachmentService attachmentService, IRenderPdfService renderPdfService)
        {
            _renderPdfService = renderPdfService;
            _productService = productService;
            _attachmentService = attachmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string? searchQuery = null)
        {
            var response = await _productService.Get(searchQuery).ToListAsync();
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateViewModel model)
        {
            var product = new Product
            {
                Id = model.Product.Id,
                Name = model.Product.Name,
                Serial = model.Product.Serial,
                Description = model.Product.Description
            };

            await _productService.UpdateAsync(product);

            if (model.DeleteAttachmentIds != null && model.DeleteAttachmentIds.Any())
            {
                var attachmentList = await _attachmentService.FindList(model.DeleteAttachmentIds).Where(x => x.ProductId == model.Product.Id).ToListAsync();
                await _attachmentService.DeleteAsync(attachmentList);
            }

            if (model.Attachments != null && model.Attachments.Any())
            {
                foreach (var attachment in model.Attachments)
                {
                    var fileName = await _attachmentService.CreatePathAsync(attachment);

                    var attachmentEntity = new Attachment
                    {
                        ProductId = model.Product.Id,
                        Name = fileName,
                    };
                    await _attachmentService.InsertAsync(attachmentEntity);
                }
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ProductCreateDto product, List<IFormFile> attachments)
        {
            if (product == null)
            {
                return BadRequest("Product data is required.");
            }
            try
            {
                var newProduct = new Product
                {
                    Name = product.Name,
                    Serial = product.Serial,
                    Description = product.Description
                };
                await _productService.InsertAsync(newProduct);

                if (attachments != null && attachments.Any())
                {
                    foreach (var attachment in attachments)
                    {
                        var fileName = await _attachmentService.CreatePathAsync(attachment);

                        var attachmentEntity = new Attachment
                        {
                            ProductId = newProduct.Id,
                            Name = fileName,
                        };
                        await _attachmentService.InsertAsync(attachmentEntity);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



        [HttpDelete]
        public async Task<IActionResult> Delete(List<int> idList)
        {
            var products = await _productService.FindOne(idList).ToListAsync();
            await _productService.DeleteAsync(products);
            return Ok();
        }

        [HttpPost("GeneratePdf")]
        public async Task<IActionResult> GeneratePdf([FromBody] GeneratePdfViewModel model)
        {
            if (model == null || model.ProductIds == null || !model.ProductIds.Any())
            {
                return BadRequest("Product IDs are required.");
            }
            try
            {
                var products = await _productService.FindOne(model.ProductIds).Include(p => p.Attachments).ToListAsync();

                if (!products.Any())
                {
                    return NotFound("No products found for the given IDs.");
                }
                var doc = _renderPdfService.GeneratePdfInvoice(products, model.User, DateTime.Today.ToShortDateString(), model.Location, model.Title);
                return File(doc, "application/pdf", $"{model.User}_zimmet.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        
    }

}
