using System.Security.Claims;
using CoreLibrary.Auth;
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
        public async Task<IActionResult> Login([Bind("Correo, Contrasena, Recordarme")] LoginViewModel usuario)
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

            await CreateClaims(usuarioAutenticado, usuario.Recordarme);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro([Bind("NombreUsuario, NombreCompleto, Correo, Telefono, Contrasena")] UsuarioModel usuario)
        {
            ModelState.Remove("Rol");

            if (!ModelState.IsValid)
            {
                return View(usuario);
            }
            var usuarioExistente = await _usuarioService.ObtenerPorCorreo(usuario.Correo);
            if (usuarioExistente != null)
            {
                ModelState.AddModelError("", "El correo ya está en uso.");
                return View(usuario);
            }

            usuario.Rol = RolesEnum.Usuario;

            await _usuarioService.Guardar(usuario);
            return RedirectToAction("Login");
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

        [HttpGet]
        public async Task<IActionResult> EditarUsuario(int id)
        {
            // Cargar los roles como una selectList
            var usuario = await _usuarioService.ObtenerPorId(id);
            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            return Ok(usuario);
        }

        [HttpGet]
        public IActionResult ObtenerRoles()
        {
            var roles = Enum.GetValues(typeof(RolesEnum))
                            .Cast<RolesEnum>()
                            .Select(r => new { Id = (int)r, Name = r.ToString() })
                            .ToList();

            return Json(roles);
        }
        [HttpPost]
        public async Task<IActionResult> ActualizarUsuario(UsuarioModel usuario)
        {
            ModelState.Remove("Contrasena");

            if (!ModelState.IsValid)
            {
                return BadRequest(new { mensaje = "Datos inválidos", errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var usuarioExistente = await _usuarioService.ObtenerPorId(usuario.Id);
            if (usuarioExistente == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            await _usuarioService.Actualizar(usuario);

            return Ok(new { mensaje = "Usuario actualizado con éxito" });
        }

        [HttpPost]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _usuarioService.ObtenerPorId(id);
            if (usuario == null)
            {
                return NotFound();
            }
            await _usuarioService.Eliminar(id);
            return RedirectToAction("GestionUsuarios");
        }
        #endregion

        #region Claims
        private async Task CreateClaims(UsuarioModel usuario, bool Recordarme)
        {
            var claims = ObtenerClaims(usuario);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = Recordarme
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
