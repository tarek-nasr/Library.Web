using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Data.Models
{
    public class BorrowTransaction
    {
        public int Id { get; set; }

        [Required]
        public DateTime BorrowedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }



    }
}
