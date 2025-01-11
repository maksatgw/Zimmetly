using Zimmetly.API.Models;

namespace Zimmetly.API.Services.Abstract
{
    public interface IRenderPdfService
    {
        byte[] GeneratePdfInvoice(List<Product> products, string user, string date, string location, string title);
    }
}
