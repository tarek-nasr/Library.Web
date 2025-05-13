using Library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    }
}
