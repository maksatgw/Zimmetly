using Zimmetly.API.Models;

namespace Zimmetly.API.Services.Abstract
{
    public interface IAttachmentService
    {
        /// <summary>
        /// Creates file for each attachment and returns its path.
        /// </summary>
        /// <param name="attachments"></param>
        /// <returns></returns>
        Task<string> CreatePathAsync(IFormFile attachments);

        Task InsertAsync(Attachment attachment);

        Task DeleteAsync(List<Attachment> attachments);

        IQueryable<Attachment> FindList(List<int> idList);
    }
}
