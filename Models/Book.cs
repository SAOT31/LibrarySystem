namespace LibrarySystem.Models;

using System.Collections.Generic;

public class Book
{
    // llave primaria
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Author { get; set; }

    // estado de disponibilidad para la logica de prestamos
    public bool IsAvailable { get; set; } = true;

    // propiedad de navegacion: un libro puede tener muchos registros de prestamo
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}