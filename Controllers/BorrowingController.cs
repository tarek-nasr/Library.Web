using Library.Data.ViewModels;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Controllers
{
    public class BorrowingController : Controller
    {
        private readonly IBorrowingService _borrowingService;
        public BorrowingController(IBorrowingService borrowingService)
        {
            _borrowingService = borrowingService;
        }



        public async Task<IActionResult> Index(string status, DateTime? borrowDate, DateTime? returnDate, int page = 1, int pageSize = 5)
        {
            var transactionsQuery = await _borrowingService.GetAllQueryableAsync();

            if (!string.IsNullOrEmpty(status))
            {
                transactionsQuery = status switch
                {
                    "borrowed" => transactionsQuery.Where(t => t.ReturnedDate == null),
                    "available" => transactionsQuery.Where(t => t.ReturnedDate != null),
                    _ => transactionsQuery
                };
            }

            if (borrowDate.HasValue)
            {
                transactionsQuery = transactionsQuery.Where(t => t.BorrowedDate.Date == borrowDate.Value.Date);
            }

            if (returnDate.HasValue)
            {
                transactionsQuery = transactionsQuery.Where(t => t.ReturnedDate.HasValue &&
                                                                 t.ReturnedDate.Value.Date == returnDate.Value.Date);
            }

            var totalCount = await transactionsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var transactions = await transactionsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(t => t.Book)
                    .ThenInclude(b => b.Author)
                .ToListAsync();

            var model = new PagedBorrowTransactionViewModel
            {
                Transactions = transactions,
                CurrentPage = page,
                TotalPages = totalPages,
                Status = status,
                BorrowDate = borrowDate,
                ReturnDate = returnDate
            };

            return View(model);
        }



        public async Task<IActionResult> Borrow()
        {
            var books = await _borrowingService.GetAvailableBooksAsync();
            ViewBag.BookList = new SelectList(books, "Id", "Title");
            return View(new BorrowViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Borrow(BorrowViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var books = await _borrowingService.GetAvailableBooksAsync();
                ViewBag.BookList = new SelectList(books, "Id", "Title");
                return View(model);
            }

            try
            {
                await _borrowingService.BorrowBookAsync(model.BookId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var books = await _borrowingService.GetAvailableBooksAsync();
                ViewBag.BookList = new SelectList(books, "Id", "Title");
                return View(model);
            }
        }

        public async Task<IActionResult> Return()
        {
            var activeTransactions = await _borrowingService.GetActiveBorrowingsAsync();
            ViewBag.ActiveBorrowings = new SelectList(activeTransactions, "Id", "BookTitle");

            return View(new ReturnViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Return(ReturnViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var active = await _borrowingService.GetActiveBorrowingsAsync();
                ViewBag.ActiveBorrowings = new SelectList(active, "Id", "BookTitle");
                return View(model);
            }

            try
            {
                await _borrowingService.ReturnBookAsync(model.TransactionId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var active = await _borrowingService.GetActiveBorrowingsAsync();
                ViewBag.ActiveBorrowings = new SelectList(active, "Id", "BookTitle");
                return View(model);
            }
        }
        [HttpGet]
        public async Task<JsonResult> CheckAvailability(int bookId)
        {
            var books = await _borrowingService.GetAvailableBooksAsync();
            var isAvailable = books.Any(b => b.Id == bookId);
            return Json(new { isAvailable });
        }





    }
}
