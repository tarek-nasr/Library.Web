using Library.Data.Models;
using Library.Data.ViewModels;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        public BooksController(IBookService bookService, IAuthorService authorService)
        {
            _bookService = bookService;
            _authorService = authorService;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllAsync();
            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Genre = book.Genre,
                Description = book.Description,
                AuthorFullName = book.Author.FullName
            };

            return View(viewModel);
        }





        private async Task PopulateAuthorsDropDown(int? selectedId = null)
        {
            var authors = await _authorService.GetAllAsync();
            ViewBag.AuthorId = new SelectList(authors, "Id", "FullName", selectedId);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateAuthorsDropDown();
            ViewBag.Genres = Enum.GetValues(typeof(Genre));
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            await _bookService.AddAsync(book);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null) return NotFound();

            await PopulateAuthorsDropDown(book.AuthorId);
            ViewBag.Genres = Enum.GetValues(typeof(Genre));
            return View(book);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            await _bookService.UpdateAsync(book);
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null) return NotFound();
            return View(book);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
