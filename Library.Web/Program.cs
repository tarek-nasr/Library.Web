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







            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<LibraryDbContext>();


                if (!context.Authors.Any())
                {
                    context.Authors.AddRange(
                        new Author { FullName = "Naguib Abdel Rahman Mahfouz", Email = "mahfouz@ex.com", Website = "www.naz.com", Bio = "Egyptian" },
                        new Author { FullName = "Ghassan Fouad Ali Kana", Email = "ghassan@ex.com", Website = "www.ghi.com", Bio = "writer" },
                        new Author { FullName = "Ahlam Omar Saeed Moste", Email = "ahlam@ex.com", Website = "www.ahla.com", Bio = "" },
                        new Author { FullName = "Youssef Kamal Ahmed Ziedan", Email = "youssef@ex.com", Website = "www.you.com", Bio = "writer" },
                        new Author { FullName = "Ibrahim Sami Nasser Nasr", Email = "ibrahim@ex.com", Website = "www.ibr.com", Bio = "Jordanian" },
                        new Author { FullName = "Tawfiq Mmoud Salim Al-Hakim", Email = "tawfiq@ex.com", Website = "www.taw.com", Bio = "playwright" },
                        new Author { FullName = "May Elias Geges Zieh", Email = "mai@exa.com", Website = "www.may.com", Bio = " writer" },
                        new Author { FullName = "Alaa Youssef Nal Aswany", Email = "alaa@exa.com", Website = "www.ala.com", Bio = "Contemporary" },
                        new Author { FullName = "Hanan Fawzi Hassan Al", Email = "hanan@exam.com", Website = "www.han.com", Bio = "Prominent " }
                    );

                    context.SaveChanges();

                    context.Books.AddRange(
                        new Book { Title = "Children of Our Alley", Genre = Genre.Drama, AuthorId = 1, Description = "A famous novel" },
                        new Book { Title = "Men in the Sun", Genre = Genre.History, AuthorId = 2, Description = "A novel" },
                        new Book { Title = "The Prophet", Genre = Genre.Romance, AuthorId = 3, Description = "A collection of poeticStory" },
                        new Book { Title = "Memory in the Flesh", Genre = Genre.Romance, AuthorId = 4, Description = "A romantic novel" },
                        new Book { Title = "Azazel", Genre = Genre.History, AuthorId = 5, Description = "" },
                        new Book { Title = "Diary of a Country Prosecutor", Genre = Genre.SciFi, AuthorId = 7, Description = "A novel" },
                        new Book { Title = "The White Rose", Genre = Genre.Poetry, AuthorId = 8, Description = "A collection of emotional" },
                        new Book { Title = "The Yacoubian Building", Genre = Genre.Drama, AuthorId = 9, Description = "A novel exposing" },
                        new Book { Title = "Woman at Point Zero", Genre = Genre.Drama, AuthorId = 10, Description = "A novel about a woman" }
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
