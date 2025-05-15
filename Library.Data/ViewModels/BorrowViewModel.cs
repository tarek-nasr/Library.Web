using System.ComponentModel.DataAnnotations;

namespace Library.Data.ViewModels
{
    public class BorrowViewModel
    {
        [Required(ErrorMessage = "Please select a book.")]
        public int BookId { get; set; }
    }
}
