using Library.Data.Models;

namespace Library.Data.ViewModels
{
    public class PagedAuthorListViewModel
    {
        public IEnumerable<Author> Authors { get; set; } = Enumerable.Empty<Author>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}
