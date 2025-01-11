using Zimmetly.API.DTOs;

namespace Zimmetly.API.ViewModels
{
    public class ProductUpdateViewModel
    {
        public ProductUpdateDto Product { get; set; } 
        public List<IFormFile>? Attachments { get; set; }
        public List<int>? DeleteAttachmentIds { get; set; }
    }
}
