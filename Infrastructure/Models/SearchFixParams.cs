using Microsoft.AspNetCore.Mvc;

namespace Infrastructure
{
    public class SearchFixParams
    {
        public string Category { get; set; }
        public string Author { get; set; }
        public string Tag { get; set; }
        public string Title { get; set; }
        public string Solution { get; set; }
        public int PageId { get; set; } = 1;
    }
}
