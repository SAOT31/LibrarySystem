namespace LibrarySystem.Services;

using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

public class BookService
{
    // conexion a la base de datos inyectada desde Program.cs
    private readonly LibraryDbContext _context;

    public BookService(LibraryDbContext context)
    {
        _context = context;
    }

  
    // inserta un nuevo libro en MySQL
    public async Task AddBookAsync(Book newBook)
    {
        _context.Books.Add(newBook);
        await _context.SaveChangesAsync(); 
    }

    
    // trae todos los registros de la tabla Books
    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _context.Books.ToListAsync();
    }

    // busca un libro por ID
    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await _context.Books.FindAsync(id);
    }

 
  
    public async Task UpdateBookAsync(Book updatedBook)
    {
        _context.Books.Update(updatedBook);
        await _context.SaveChangesAsync();
    }

   
    // busca el registro y lo remueve si existe para evitar excepciones
    public async Task DeleteBookAsync(int id)
    {
        var bookToDelete = await GetBookByIdAsync(id);
        
        if (bookToDelete != null)
        {
            _context.Books.Remove(bookToDelete);
            await _context.SaveChangesAsync();
        }
    }
}