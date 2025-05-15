using Library.Data.Models;
using Library.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Services.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly LibraryDbContext _context;
        public AuthorService(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors.Include(a => a.Books).ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _context.Authors.Include(a => a.Books)
                 .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
        }



        public async Task<bool> IsNameUniqueAsync(string fullName, int? excludeId = null)
        {

            return !await _context.Authors
                  .AnyAsync(a => a.FullName == fullName && (!excludeId.HasValue || a.Id != excludeId));

        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null)
        {
            return !await _context.Authors
                  .AnyAsync(a => a.Email == email && (!excludeId.HasValue || a.Id != excludeId));
        }

    }
}
