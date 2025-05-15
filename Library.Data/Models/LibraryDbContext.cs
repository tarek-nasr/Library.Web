using Microsoft.EntityFrameworkCore;

namespace Library.Data.Models
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowTransaction> BorrowTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
               .HasIndex(a => a.FullName)
               .IsUnique();

            modelBuilder.Entity<Author>()
                .HasIndex(a => a.Email)
                .IsUnique();
        }
    }


}
