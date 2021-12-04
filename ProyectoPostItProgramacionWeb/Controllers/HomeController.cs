using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoPostItProgramacionWeb.Helpers;
using ProyectoPostItProgramacionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProyectoPostItProgramacionWeb.Controllers
{
    public class HomeController : Controller
    {
        public postitdbContext Context { get; }

        public HomeController(postitdbContext context)
        {
            Context = context;
        }
        public IActionResult Index()
        {
            var notas = (IEnumerable<Nota>)Context.Nota.Include(x => x.IdMazoNavigation).ThenInclude(x => x.IdUsuarioNavigation).Select(x =>x).Where(x=> x.IdMazoNavigation.IdUsuario == 1 && x.IdMazo == 1).ToList();
            return View(notas);
       
        }
        [HttpGet("Usuario/Registrarse/")]
        public IActionResult RegistrarUsuario()
        {
            return View();
        }
        [HttpPost("Usuario/Registrarse/")]
        public IActionResult RegistrarUsuario(Usuario usuario)
        {
            usuario.Password = Cifrado.GetHash(usuario.Password);
            Context.Add(usuario);
            Context.SaveChanges();
            return View();
        }
        [HttpPost("Usuario/IniciarSesion")]
        public IActionResult IniciarSesionUsuario()
        {
            return View();
        }
        [HttpPost("Usuario/IniciarSesion")]
        public async Task<IActionResult> IniciarSesionUsuario(Usuario usuarioent)
        {
            var usuario = Context.Usuario.SingleOrDefault(x => x.Nombre == usuarioent.Nombre && x.Password == Cifrado.GetHash(usuarioent.Password));
            if (usuario!=null)
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, usuario.Nombre));
                claims.Add(new Claim("Id", User.Identity.ToString()));
                var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(new ClaimsPrincipal(identidad));
                return RedirectToAction("Index", "Home", new { area = "Usuario" });
            }
            else
            {
                ModelState.AddModelError("", "El usuario o contraseña son incorrectos");
               return View();
            }
         
        }
        [Route("Usuario/AccesoDenegado")]
        public IActionResult AccesoDenegado()
        {
            return View();
        }

    }
}
