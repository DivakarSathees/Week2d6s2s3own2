using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string Author { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int PublishedYear { get; set; }

        public int LibraryCardId { get; set; }

        public LibraryCard LibraryCard { get; set; } // Navigation property
    }
}
