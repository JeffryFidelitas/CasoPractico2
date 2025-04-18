using System.Security.Claims;
using CoreLibrary.Models;
using CoreLibrary.Models.ViewModels;
using CoreLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace EventCorp.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        #region Autenticacion

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Correo, Contrasena")] LoginViewModel usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            var usuarioAutenticado = await _usuarioService.ObtenerPorCorreoYContrasena(usuario.Correo, usuario.Contrasena);

            if (usuarioAutenticado == null)
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                return View(usuario);
            }

            await CreateClaims(usuarioAutenticado);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region CRUD
        [HttpGet]
        public async Task<IActionResult> GestionUsuarios()
        {
            var usuarios = await _usuarioService.ObtenerTodos();
            return View(usuarios);
        }
        #endregion

        #region Claims
        private async Task CreateClaims(UsuarioModel usuario)
        {
            var claims = ObtenerClaims(usuario);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        private List<Claim> ObtenerClaims(UsuarioModel usuario)
        {
            return new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario!),
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()!),
                    new Claim(ClaimTypes.Email, usuario.Correo!),
                    new Claim(ClaimTypes.Role, usuario.Rol.ToString()!)
                };
        }
        #endregion
    }
}
