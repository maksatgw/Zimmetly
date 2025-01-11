using System.Text.Json.Serialization;

namespace Zimmetly.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Serial { get; set; }
        public string? Description { get; set; }
        public List<Attachment>? Attachments { get; set; }
    }
}
