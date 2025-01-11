using Microsoft.EntityFrameworkCore;
using Zimmetly.API.Context;
using Zimmetly.API.Models;
using Zimmetly.API.Services.Abstract;

namespace Zimmetly.API.Services.Concrete
{
    public class AttachmentService : IAttachmentService
    {
        private readonly AppDbContext _context;

        public AttachmentService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<string> CreatePathAsync(IFormFile attachment)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(attachment.FileName)}";
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await attachment.CopyToAsync(stream);
            }
            return fileName;
        }

        public async Task DeleteAsync(List<Attachment> attachments)
        {
            _context.Attachments.RemoveRange(attachments);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Attachment> FindList(List<int> idList)
        {
            if (idList == null || !idList.Any())
            {
                throw new ArgumentException("The ID list cannot be null or empty.", nameof(idList));
            }
            try
            {
                var query = _context.Attachments
                    .Where(p => idList.Contains(p.Id));
                return query;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in Find: {ex.Message}");
                throw new Exception("An error occurred while finding the products.", ex);
            }
        }

        public async Task<Attachment> GetByIdAsync(int id)
        {
            try
            {
                var attachment = await _context.Attachments.FirstOrDefaultAsync(x => x.Id == id);

                if (attachment == null)
                {
                    throw new KeyNotFoundException($"No attachment found with ID {id}.");
                }

                return attachment;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the attachment.", ex);
            }
        }


        public async Task InsertAsync(Attachment attachment)
        {
            await _context.Attachments.AddAsync(attachment);
            await _context.SaveChangesAsync();
        }
    }
}
