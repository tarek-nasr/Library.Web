using Library.Data.Models;

namespace Library.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author?> GetByIdAsync(int id);
        Task AddAsync(Author author);
        Task UpdateAsync(Author author);
        Task DeleteAsync(int id);
        Task<bool> IsNameUniqueAsync(string fullName, int? excludeId = null);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null);
    }
}
