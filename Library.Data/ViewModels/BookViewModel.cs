using Library.Data.Models;

namespace Library.Data.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Genre Genre { get; set; }
        public string Description { get; set; }
        public string AuthorFullName { get; set; }
    }
}
