﻿using Microsoft.AspNetCore.Mvc;
using Desarrollo.Models;
using Desarrollo.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Desarrollo.Controllers;

public class UsuariosController : Controller
{
    public readonly DesarrolloDbContext _context;

    public UsuariosController(DesarrolloDbContext context)
    {
        _context = context;
    }
    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registro(string Email, string Contraseña, bool Mantener_sesion)
    {
        // Verificar si el usuario ya existe
        var existeUsuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == Email);
        if (existeUsuario != null)
        {
            Console.WriteLine("Usuario encontrado con el email: " + Email);
            ModelState.AddModelError("Email", "Este Correo ya está en uso.");
            return View();
        }

        // Encriptar la contraseña
        var contraseñaEncriptada = BCrypt.Net.BCrypt.HashPassword(Contraseña);
        var usuario = new Usuario
        {
            Email = Email,
            Contraseña = contraseñaEncriptada
        };

        try
        {
            _context.Add(usuario);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Manejar el error, por ejemplo, loguearlo
            ModelState.AddModelError("", "No se pudo registrar el usuario: " + ex.Message);
            return View();
        }

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = Mantener_sesion
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

        // Redirigir a la vista de inicio
        return RedirectToAction("Index", "Home");
    }


    public async Task<IActionResult> CerrarSesion()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult IniciarSesion()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IniciarSesion(string Email, string Contraseña, bool Mantener_sesion)
    {
        var existeUsuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == Email);
        
        if (existeUsuario == null)
        {
            ModelState.AddModelError("Email", "EL correo no está registrado, tiene que registrarse");
            return View();
        }

        bool contraseñaValida = BCrypt.Net.BCrypt.Verify(Contraseña, existeUsuario.Contraseña);

        if (!contraseñaValida)
        {
            ModelState.AddModelError("Contraseña", "La contraseña es incorreccta");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, existeUsuario.Email),
            new Claim(ClaimTypes.Email,existeUsuario.Email)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = Mantener_sesion
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

        return RedirectToAction("Index", "Home");
    }
}
