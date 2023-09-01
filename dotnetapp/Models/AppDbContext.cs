// using Microsoft.EntityFrameworkCore;
// using dotnetapp.Models;

// namespace dotnetapp.Models
// {
//     public class AppDbContext : DbContext
//     {
//         public DbSet<Book> Books { get; set; }
//         public DbSet<LibraryCard> LibraryCards { get; set; }

//         public AppDbContext(DbContextOptions<AppDbContext> options)
//             : base(options)
//         {
//         }

//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             modelBuilder.Entity<Book>()
//                 .HasOne(b => b.LibraryCard)
//                 .WithOne(lc => lc.Book)
//                 .HasForeignKey<Book>(b => b.LibraryCardId); // Use the appropriate property name

//             modelBuilder.Entity<LibraryCard>()
//                 .HasOne(lc => lc.Book)
//                 .WithOne(b => b.LibraryCard)
//                 .HasForeignKey<LibraryCard>(lc => lc.BookId); // Use the appropriate property name

//             // Other configurations

//             base.OnModelCreating(modelBuilder);
//         }
//     }
// }

using Microsoft.EntityFrameworkCore;
using dotnetapp.Models;

namespace dotnetapp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<LibraryCard> LibraryCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.LibraryCard)
                .WithOne(lc => lc.Book)
                .HasForeignKey<Book>(b => b.LibraryCardId);

            // modelBuilder.Entity<LibraryCard>()
            //     .HasOne(lc => lc.Book)
            //     .WithOne(b => b.LibraryCard)
            //     .HasForeignKey<LibraryCard>(lc => lc.BookId); // Use the appropriate property name


            // Other configurations

            base.OnModelCreating(modelBuilder);
        }
    }
}


