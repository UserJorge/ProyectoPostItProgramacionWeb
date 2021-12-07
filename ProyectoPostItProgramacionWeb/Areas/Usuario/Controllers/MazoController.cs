using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoPostItProgramacionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoPostItProgramacionWeb.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize]
    public class MazoController : Controller
    {
        //Mostrar la lista de mazos del usuario
   
        public postitdbContext Context { get; }
        public IWebHostEnvironment Host { get; }

        public MazoController(postitdbContext context, IWebHostEnvironment host)
        {
            Context = context;
            Host = host;
        }
       [HttpGet("Usuario/Usuario/Mazo")]
      
        public IActionResult Index()
        {
            var mazos =(IEnumerable<ProyectoPostItProgramacionWeb.Models.Mazo>)Context.Mazo.Include(x => x.IdUsuarioNavigation).Select(x => x).Where(x => x.IdUsuarioNavigation.Nombre == User.Identity.Name).ToList();
            return View(mazos);
        }
        [HttpGet("Usuario/Usuario/AgregarMazo")]
        public IActionResult AgregarMazo()
        {

            return View();
        }
        [HttpGet("Usuario/Usuario/AgregarMazo")]
        public IActionResult AgregarMazo(ProyectoPostItProgramacionWeb.Models.Mazo mazo)
        {
            if (mazo!=null)
            {
                Context.Mazo.Add(mazo);
                Context.SaveChanges();
            }
            return View();
        }
        public IActionResult EditarMazo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult EditarMazo(Models.Mazo mazo)
        {
            if (mazo!=null)
            {             
                Context.Mazo.Update(mazo);
                Context.SaveChanges();
            }
            return View();
        }
        public IActionResult EliminarMazo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult EliminarMazo(Models.Mazo mazo)
        {
            if (mazo == null)
            {
                ModelState.AddModelError("", "El mazo no puede ser eliminado por que no existe");
                return View(mazo);
            }
            if (string.IsNullOrWhiteSpace(mazo.Titulo))
            {
                ModelState.AddModelError("", "El mazo no puede ser eliminado por que no existe");
                return View(mazo);
            }
            var mazodb = Context.Mazo.FirstOrDefault(x => x.Titulo == mazo.Titulo);           
            Context.Remove(mazodb);
            Context.SaveChanges();
            return View();
        }
    }
}
