using Library.Data.Models;
using Library.Data.ViewModels;
using Library.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Services.Services
{
    public class BorrowingService : IBorrowingService
    {
        private readonly LibraryDbContext _context;
        public BorrowingService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BorrowTransaction>> GetAllAsync()
        {
            return await _context.BorrowTransactions
            .Include(bt => bt.Book)
            .ThenInclude(b => b.Author)
            .ToListAsync();
        }
        public async Task<IQueryable<BorrowTransaction>> GetAllQueryableAsync()
        {
            return _context.BorrowTransactions
                .Include(bt => bt.Book)
                .ThenInclude(b => b.Author)
                .AsQueryable();
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _context.Books
             .Include(b => b.BorrowTransactions)
             .Where(b => !b.BorrowTransactions.Any(t => t.ReturnedDate == null))
             .ToListAsync();
        }


        public async Task BorrowBookAsync(int bookId)
        {
            var book = await _context.Books
              .Include(b => b.BorrowTransactions)
              .FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null || book.IsBorrowed)
                throw new InvalidOperationException("Book is not available.");

            var transaction = new BorrowTransaction
            {
                BookId = bookId,
                BorrowedDate = DateTime.UtcNow
            };

            _context.BorrowTransactions.Add(transaction);
            await _context.SaveChangesAsync();
        }



        public async Task ReturnBookAsync(int transactionId)
        {
            var transaction = await _context.BorrowTransactions
                .FirstOrDefaultAsync(t => t.Id == transactionId && t.ReturnedDate == null);

            if (transaction == null)
                throw new InvalidOperationException("Transaction not found or already returned.");

            transaction.ReturnedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<ActiveBorrowingViewModel>> GetActiveBorrowingsAsync()
        {

            var activeTransactions = await _context.BorrowTransactions
                .Where(bt => bt.ReturnedDate == null)
                .Include(bt => bt.Book)
                .ToListAsync();


            var activeBorrowings = activeTransactions.Select(bt => new ActiveBorrowingViewModel
            {
                Id = bt.Id,
                BookTitle = bt.Book.Title
            }).ToList();

            return activeBorrowings;
        }



    }
}
