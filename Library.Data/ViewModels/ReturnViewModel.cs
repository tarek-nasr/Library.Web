using System.ComponentModel.DataAnnotations;

namespace Library.Data.ViewModels
{
    public class ReturnViewModel
    {
        [Required(ErrorMessage = "Please select a book.")]
        public int TransactionId { get; set; }
    }
}
