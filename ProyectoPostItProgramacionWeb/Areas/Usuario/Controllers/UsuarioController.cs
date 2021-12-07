using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoPostItProgramacionWeb.Models;
using ProyectoPostItProgramacionWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoPostItProgramacionWeb.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize]
    public class UsuarioController : Controller
    {
        public postitdbContext Context { get; }

        public UsuarioController(postitdbContext context)
        {
            Context = context;
        }
       
        [HttpGet("Usuario/Usuario/Index")]
        [Authorize]
        public IActionResult Index()
        {
            var usuarios =(IEnumerable<ProyectoPostItProgramacionWeb.Models.Usuario>) Context.Usuario.Select(x => x).Where(x => x.Nombre != "admin").ToList();
            return View(usuarios);
        }
       [HttpGet("Usuario/Usuario/Salir")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("~/");
        }
        [Authorize]
        [HttpPost("Usuario/Eliminar/")]
        public IActionResult EliminarUsuario(Models.Usuario usuario)
        {
            var user = Context.Usuario.FirstOrDefault(X => X.Nombre == usuario.Nombre);
            if (user!=null)
            {
                Context.Remove(user);
                Context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
