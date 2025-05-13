using Library.Data.Models;
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
        public async Task<IActionResult> Index()
        {
            var authors = await _authorService.GetAllAsync();
            return View(authors);
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

            bool isUnique = await _authorService.IsNameUniqueAsync(author.FullName);
            if (!isUnique)
            {
                ModelState.AddModelError("FullName", "An author with this name already exists.");
                return View(author);
            }

            await _authorService.AddAsync(author);
            return RedirectToAction(nameof(Index));
        }





    }
}
