using Library.Data.Models;
using Library.Services.Interfaces;
using Library.Services.Services;
using Microsoft.EntityFrameworkCore;

namespace Library.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IAuthorService, AuthorService>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IBorrowingService, BorrowingService>();

            builder.Services.AddDbContext<LibraryDbContext>(options =>
                options.UseInMemoryDatabase("LibraryDb"));


            var app = builder.Build();






            // Add after building the app
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<LibraryDbContext>();

                // Seed data
                if (!context.Authors.Any())
                {
                    context.Authors.AddRange(
                        new Author { FullName = "John Ronald Reuel Tolkien", Email = "tolkien@example.com", Website = "www.tolkienestate.com", Bio = "English writer, poet, philologist, and academic." },
                        new Author { FullName = "Joanne Kathleen Rowling", Email = "rowling@example.com", Website = "www.jkrowling.com", Bio = "British author and philanthropist." }
                    );
                    context.SaveChanges();

                    context.Books.AddRange(
                        new Book { Title = "The Hobbit", Genre = Genre.Fantasy, AuthorId = 1, Description = "Fantasy novel about Bilbo Baggins." },
                        new Book { Title = "Harry Potter", Genre = Genre.Fantasy, AuthorId = 2, Description = "Story of a young wizard." }
                    );
                    context.SaveChanges();
                }
            }




            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
