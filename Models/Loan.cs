namespace LibrarySystem.Models;

public class Loan
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public int UserId { get; set; }

    public DateTime LoanDate { get; set; } = DateTime.Now;

    public DateTime? ReturnDate { get; set; }

    // propiedades de navegacion necesarias para Entity Framework
    public Book? Book { get; set; }
    public User? User { get; set; }
}