using System.Text.Json.Serialization;

namespace Zimmetly.API.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
    }
}
