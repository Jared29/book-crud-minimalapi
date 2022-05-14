using Microsoft.EntityFrameworkCore;

namespace BookManager.Models {
    public class Book {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
    }

    class BookDb : DbContext {
        public BookDb(DbContextOptions options) : base(options) { }
        public DbSet<Book> Books {get; set; }
    }
}