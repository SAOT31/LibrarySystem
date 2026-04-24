namespace LibrarySystem.Services;

using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

public class LoanService
{
    private readonly LibraryDbContext _context;

    public LoanService(LibraryDbContext context)
    {
        _context = context;
    }

    // consulta prestamos incluyendo datos de tablas relacionadas (Book y User)
    public async Task<List<Loan>> GetAllLoansAsync()
    {
        return await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.User)
            .ToListAsync();
    }

    // busca prestamo por id incluyendo sus relaciones
    public async Task<Loan?> GetLoanByIdAsync(int id)
    {
        return await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    // valida disponibilidad, multas y registra el prestamo
    public async Task<string> BorrowBookAsync(int userId, int bookId)
    {
        var user = await _context.Users.FindAsync(userId);
        var book = await _context.Books.FindAsync(bookId);

        if (user == null || book == null) return "User or Book not found.";

        if (!book.IsAvailable) return "The book is already borrowed.";

        if (user.HasPenalties) return "User has active penalties.";

        // actualiza estado del libro a no disponible
        book.IsAvailable = false;
        _context.Books.Update(book);

        var newLoan = new Loan
        {
            UserId = userId,
            BookId = bookId,
            LoanDate = DateTime.Now
        };

        _context.Loans.Add(newLoan);
        await _context.SaveChangesAsync();
        return "Loan registered successfully.";
    }

    // procesa devolucion y libera el libro para nuevos prestamos
    public async Task<string> ReturnBookAsync(int loanId)
    {
        var loan = await _context.Loans.FindAsync(loanId);
        if (loan == null) return "Loan record not found.";

        var book = await _context.Books.FindAsync(loan.BookId);
        if (book != null)
        {
            book.IsAvailable = true;
            _context.Books.Update(book);
        }

        loan.ReturnDate = DateTime.Now;
        _context.Loans.Update(loan);
        await _context.SaveChangesAsync();
        
        return "Book returned successfully.";
    }

    // actualiza datos generales del prestamo
    public async Task UpdateLoanAsync(Loan updatedLoan)
    {
        _context.Loans.Update(updatedLoan);
        await _context.SaveChangesAsync();
    }

    // remueve el registro de la base de datos
    public async Task DeleteLoanAsync(int id)
    {
        var loanToDelete = await _context.Loans.FindAsync(id);
        if (loanToDelete != null)
        {
            _context.Loans.Remove(loanToDelete);
            await _context.SaveChangesAsync();
        }
    }
}