namespace LibrarySystem.Controllers;

using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Services;
using LibrarySystem.Models;
using LibrarySystem.Responsive;

public class UserController : Controller
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    // Lista los usuarios y los formatea con UserResponse para la vista
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllUsersAsync();
        
        var response = users.Select(u => new UserResponse
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            // Convertimos el booleano en un texto amigable para el administrador
            PenaltyStatus = u.HasPenalties ? "Suspended" : "Clear"
        }).ToList();

        return View(response);
    }

    //  Retorna la vista con el formulario de registro
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    //  Recibe los datos y persiste el nuevo usuario en la base de datos
    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        if (ModelState.IsValid)
        {
            await _userService.AddUserAsync(user);
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    //  Obtiene el usuario actual para cargar sus datos en el formulario
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    //  Aplica los cambios recibidos sobre el registro existente
    [HttpPost]
    public async Task<IActionResult> Edit(User user)
    {
        if (ModelState.IsValid)
        {
            await _userService.UpdateUserAsync(user);
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    //  Ejecuta la eliminacion del usuario segun su identificador
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.DeleteUserAsync(id);
        return RedirectToAction(nameof(Index));
    }
}