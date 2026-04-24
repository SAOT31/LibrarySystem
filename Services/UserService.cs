namespace LibrarySystem.Services;

using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;

public class UserService
{
    private readonly LibraryDbContext _context;

    // inyeccion de la conexion a la base de datos
    public UserService(LibraryDbContext context)
    {
        _context = context;
    }

   
    // registra un nuevo usuario en la tabla Users
    public async Task AddUserAsync(User newUser)
    {
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
    }

    
    // obtiene el listado completo de usuarios
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    // busca un usuario por su llave primaria (Id)
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    
    // actualiza los datos del usuario (Nombre, Email, Multas)
    public async Task UpdateUserAsync(User updatedUser)
    {
        _context.Users.Update(updatedUser);
        await _context.SaveChangesAsync();
    }

    
    // verifica existencia y remueve el registro de MySQL
    public async Task DeleteUserAsync(int id)
    {
        var userToDelete = await GetUserByIdAsync(id);
        
        if (userToDelete != null)
        {
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }
    }
}