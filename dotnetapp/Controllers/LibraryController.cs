using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
// using dotnetapp.Data; // Import your DbContext namespace
using dotnetapp.Models;

namespace dotnetapp.Controllers
{
    public class LibraryController : Controller
    {
        private readonly AppDbContext _context;

        public LibraryController(AppDbContext context)
        {
            _context = context;
        }

        // Implement a method to display books associated with a library card.
        public IActionResult DisplayBooksForLibraryCard(int libraryCardId)
{
    var libraryCard = _context.LibraryCards.FirstOrDefault(lc => lc.Id == libraryCardId);

    if (libraryCard == null)
    {
        // Handle the case where the library card with the given ID doesn't exist.
        return NotFound(); // Return a 404 Not Found response or handle it as needed.
    }

    var books = _context.Books
        .Where(b => b.LibraryCardId == libraryCardId)
        .ToList();

    return View(books);
}


        // Implement a method to display all books in the library.
        public IActionResult DisplayAllBooks()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        // Implement a method to search for books by title.
        public IActionResult SearchBooksByTitle(string query)
        {
            query = query?.Trim() ?? ""; // Trim and handle null query
            var books = _context.Books
                .Where(b => b.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return View(books);
        }

        // Implement a method to get available books.
        public IActionResult GetAvailableBooks()
        {
            var availableBooks = _context.Books
                .Where(b => b.LibraryCardId == null) // Books that are not borrowed
                .ToList();

            return View(availableBooks);
        }

        // Implement a method to get borrowed books.
        public IActionResult GetBorrowedBooks()
        {
            var borrowedBooks = _context.Books
                .Where(b => b.LibraryCardId != null) // Books that are borrowed
                .ToList();

            return View(borrowedBooks);
        }
    }
}
