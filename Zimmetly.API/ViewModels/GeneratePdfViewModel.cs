namespace Zimmetly.API.ViewModels
{
    public class GeneratePdfViewModel
    {
        public List<int> ProductIds { get; set; } = new List<int>();
        public string User { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
    }
}
