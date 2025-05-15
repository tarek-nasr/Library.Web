using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Data.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }



        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Author Author { get; set; }

        public ICollection<BorrowTransaction> BorrowTransactions { get; set; } = new List<BorrowTransaction>();


        [NotMapped]
        public bool IsBorrowed
        {
            get
            {
                return BorrowTransactions.Any(b => b.ReturnedDate == null);
            }
        }

    }

}
