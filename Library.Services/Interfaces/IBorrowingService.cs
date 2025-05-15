using Library.Data.Models;
using Library.Data.ViewModels;

namespace Library.Services.Interfaces
{
    public interface IBorrowingService
    {
        Task<IEnumerable<BorrowTransaction>> GetAllAsync();
        Task<IQueryable<BorrowTransaction>> GetAllQueryableAsync();
        Task<IEnumerable<Book>> GetAvailableBooksAsync();
        Task<IEnumerable<ActiveBorrowingViewModel>> GetActiveBorrowingsAsync();
        Task BorrowBookAsync(int bookId);
        Task ReturnBookAsync(int transactionId);
    }
}
