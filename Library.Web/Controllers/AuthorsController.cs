using Library.Data.Models;
using Library.Data.ViewModels;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }


        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            var allAuthors = await _authorService.GetAllAsync();
            var totalAuthors = allAuthors.Count();
            var totalPages = (int)Math.Ceiling(totalAuthors / (double)pageSize);

            var authorsOnPage = allAuthors
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new PagedAuthorListViewModel
            {
                Authors = authorsOnPage,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Author author)
        {
            if (!ModelState.IsValid) return View(author);

            bool isNameUnique = await _authorService.IsNameUniqueAsync(author.FullName);
            if (!isNameUnique)
            {
                ModelState.AddModelError("FullName", "An author with this name already exists.");
                return View(author);
            }

            bool isEmailUnique = await _authorService.IsEmailUniqueAsync(author.Email);
            if (!isEmailUnique)
            {
                ModelState.AddModelError("Email", "An author with this email already exists.");
                return View(author);
            }

            await _authorService.AddAsync(author);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Author author)
        {
            if (!ModelState.IsValid) return View(author);

            bool isNameUnique = await _authorService.IsNameUniqueAsync(author.FullName, author.Id);
            if (!isNameUnique)
            {
                ModelState.AddModelError("FullName", "An author with this name already exists.");
                return View(author);
            }

            bool isEmailUnique = await _authorService.IsEmailUniqueAsync(author.Email, author.Id);
            if (!isEmailUnique)
            {
                ModelState.AddModelError("Email", "An author with this email already exists.");
                return View(author);
            }

            await _authorService.UpdateAsync(author);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _authorService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
