namespace LibrarySystem.Controllers;

using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Services;
using LibrarySystem.Models;
using LibrarySystem.Responsive;

public class LoanController : Controller
{
    private readonly LoanService _loanService;
    private readonly BookService _bookService;
    private readonly UserService _userService;

    public LoanController(LoanService loanService, BookService bookService, UserService userService)
    {
        _loanService = loanService;
        _bookService = bookService;
        _userService = userService;
    }

    // lista los prestamos mapeando los nombres reales de libros y usuarios
    public async Task<IActionResult> Index()
    {
        var loans = await _loanService.GetAllLoansAsync();
        
        var response = loans.Select(l => new LoanResponse
        {
            Id = l.Id,
            // usamos las propiedades de navegacion para obtener los nombres
            BookTitle = l.Book?.Title ?? "Unknown",
            UserName = l.User?.FullName ?? "Unknown",
            LoanDate = l.LoanDate.ToString("yyyy-MM-dd"),
            ReturnDate = l.ReturnDate?.ToString("yyyy-MM-dd") ?? "Pending"
        }).ToList();

        return View(response);
    }

    // carga listas de seleccion para el formulario de creacion
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Books = await _bookService.GetAllBooksAsync();
        ViewBag.Users = await _userService.GetAllUsersAsync();
        return View();
    }

    // procesa el registro del prestamo y captura mensajes de validacion
    [HttpPost]
    public async Task<IActionResult> Create(int userId, int bookId)
    {
        var result = await _loanService.BorrowBookAsync(userId, bookId);
        TempData["Message"] = result;
        return RedirectToAction(nameof(Index));
    }

    // carga el prestamo y las listas de referencia para edicion
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var loan = await _loanService.GetLoanByIdAsync(id);
        if (loan == null) return NotFound();

        ViewBag.Books = await _bookService.GetAllBooksAsync();
        ViewBag.Users = await _userService.GetAllUsersAsync();
        return View(loan);
    }

    // persiste los cambios manuales en un registro de prestamo
    [HttpPost]
    public async Task<IActionResult> Edit(Loan loan)
    {
        if (ModelState.IsValid)
        {
            await _loanService.UpdateLoanAsync(loan);
            return RedirectToAction(nameof(Index));
        }
        return View(loan);
    }

    // dispara el proceso de devolucion y actualiza disponibilidad
    [HttpPost]
    public async Task<IActionResult> ReturnBook(int id)
    {
        var result = await _loanService.ReturnBookAsync(id);
        TempData["Message"] = result;
        return RedirectToAction(nameof(Index));
    }

    // elimina el registro del prestamo seleccionado
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _loanService.DeleteLoanAsync(id);
        return RedirectToAction(nameof(Index));
    }
}