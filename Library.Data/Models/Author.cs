using System.ComponentModel.DataAnnotations;

namespace Library.Data.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [RegularExpression(@"^(?:\b\w{2,}\b\s){3}\b\w{2,}\b$", ErrorMessage = "Full name must be four words, each with at least 2 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        public string? Website { get; set; }

        [StringLength(300)]
        public string? Bio { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();


    }
}
