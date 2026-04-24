namespace LibrarySystem.Controllers;

using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Services;
using LibrarySystem.Models;
using LibrarySystem.Responsive;

public class BookController : Controller
{
    private readonly BookService _bookService;

    public BookController(BookService bookService)
    {
        _bookService = bookService;
    }

    //  Lista todos los libros transformados a BookResponse
    public async Task<IActionResult> Index()
    {
        var books = await _bookService.GetAllBooksAsync();
        var response = books.Select(b => new BookResponse
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            IsAvailable = b.IsAvailable ? "Available" : "Borrowed"
        }).ToList();

        return View(response);
    }

    //  Muestra el formulario para un nuevo libro
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Recibe y guarda el nuevo libro en la base de datos
    [HttpPost]
    public async Task<IActionResult> Create(Book book)
    {
        if (ModelState.IsValid)
        {
            await _bookService.AddBookAsync(book);
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

    // Carga los datos de un libro existente para editarlos
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }

    // Procesa los cambios realizados en el libro
    [HttpPost]
    public async Task<IActionResult> Edit(Book book)
    {
        if (ModelState.IsValid)
        {
            await _bookService.UpdateBookAsync(book);
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

    // Elimina el registro por completo de la base de datos
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _bookService.DeleteBookAsync(id);
        return RedirectToAction(nameof(Index));
    }
}