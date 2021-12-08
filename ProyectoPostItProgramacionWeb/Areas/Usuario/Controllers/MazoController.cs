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
        [HttpPost("Usuario/Usuario/AgregarMazo")]
        public IActionResult AgregarMazo(ProyectoPostItProgramacionWeb.Models.Mazo mazo)
        {
            if (Context.Mazo.Any(x => x.Titulo == mazo.Titulo))
            {
                ModelState.AddModelError("", "No se puede crear otro mazo con el mismo titulo");
                return View(mazo);
            }
            if (mazo!=null)
            {
                var usuario = Context.Usuario.FirstOrDefault(x => x.Nombre == User.Identity.Name);
                mazo.IdUsuario = usuario.Id;
                Context.Mazo.Add(mazo);
                Context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [HttpGet("Usuario/Usuario/EditarMazo")]
        public IActionResult EditarMazo()
        {
            return View();
        }
        [HttpPost("Usuario/Usuario/EditarMazo")]
        public IActionResult EditarMazo(Models.Mazo mazo)
        {
            //si hay una modificación de que el mazo tenga notas, no va a ser posible la edición.
            //Ubicar el usuario
            //En que mazo lo vamos a editar
            if (mazo!=null)
            {             
                Context.Mazo.Update(mazo);
                Context.SaveChanges();
            }
            
            return View();
        }
        [HttpGet("Usuario/Usuario/EliminarMazo")]
        public IActionResult EliminarMazo()
        {
            return View();
        }
        [HttpPost("Usuario/Usuario/EliminarMazo")]
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
            if (mazodb!=null)
            {
                ModelState.AddModelError("", "El mazo no puede ser eliminado por que no existe o no se encuentra");
                return View(mazo);
            }
            if (Context.Nota.Include(x=>x.IdMazoNavigation).Any(x=>x.IdMazo==mazodb.Id))
            {
                ModelState.AddModelError("", "No se puede eliminar un mazo que tiene notas");
                return View(mazo);
            }
            Context.Remove(mazodb);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
