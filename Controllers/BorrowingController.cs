using Library.Data.ViewModels;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Controllers
{
    public class BorrowingController : Controller
    {
        private readonly IBorrowingService _borrowingService;
        public BorrowingController(IBorrowingService borrowingService)
        {
            _borrowingService = borrowingService;
        }
        public async Task<IActionResult> Index()
        {
            var transactions = await _borrowingService.GetAllAsync();
            return View(transactions);
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
                return View(model);
            }
        }

    }
}
