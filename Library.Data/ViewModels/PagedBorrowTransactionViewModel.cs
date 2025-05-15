using Library.Data.Models;

namespace Library.Data.ViewModels
{
    public class PagedBorrowTransactionViewModel
    {
        public IEnumerable<BorrowTransaction> Transactions { get; set; } = Enumerable.Empty<BorrowTransaction>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }


        public string? Status { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
