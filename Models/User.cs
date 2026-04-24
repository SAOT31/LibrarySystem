namespace LibrarySystem.Models;

using System.Collections.Generic;

public class User
{
    // llave primaria
    public int Id { get; set; }

    public required string FullName { get; set; }

    public required string Email { get; set; }

    // estado de morosidad para validaciones de negocio
    public bool HasPenalties { get; set; } = false;

    // propiedad de navegacion: un usuario puede tener muchos prestamos
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}