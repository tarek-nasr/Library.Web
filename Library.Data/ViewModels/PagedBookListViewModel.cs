using Library.Data.Models;

namespace Library.Data.ViewModels
{
    public class PagedBookListViewModel
    {
        public IEnumerable<Book> Books { get; set; } = Enumerable.Empty<Book>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
