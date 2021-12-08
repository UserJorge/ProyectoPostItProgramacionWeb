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
            var mazos=(IEnumerable<ProyectoPostItProgramacionWeb.Models.Mazo>)Context.Mazo.Include(x => x.IdUsuarioNavigation).Select(x => x).Where(x => x.IdUsuarioNavigation.Nombre == User.Identity.Name).ToList();
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
        [HttpGet("Usuario/Usuario/EditarMazo/{id}")]
        public IActionResult EditarMazo(int id)
        {
            var mazo = Context.Mazo.FirstOrDefault(x => x.Id == id);
            return View(mazo);
        }
        [HttpPost("Usuario/Usuario/EditarMazo")]
        public IActionResult EditarMazo(Models.Mazo mazo)
        {
            //si hay una modificación de que el mazo tenga notas, no va a ser posible la edición.
            //Ubicar el usuario
            //En que mazo lo vamos a editar
            if (mazo!=null)
            {
                if (!Context.Nota.Include(x=>x.IdMazoNavigation).Any(x=>x.IdMazo==mazo.Id))
                {
                    mazo.IdUsuario = Context.Mazo.Include(x => x.IdUsuarioNavigation).FirstOrDefault(x => x.IdUsuarioNavigation.Nombre == User.Identity.Name).IdUsuario;
                    Context.Mazo.Update(mazo);
                    Context.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "No se puede modificar un mazo que tiene notas");
                    return View(mazo);
                }             
            }
            else
            {
                ModelState.AddModelError("", "No debe estar vacía la información del mazo");
                return View(mazo);
            }
            return RedirectToAction("Index");
        }
        [HttpGet("Usuario/Usuario/EliminarMazo/{id}")]
        public IActionResult EliminarMazo(int id)
        {
            var mazo = Context.Mazo.FirstOrDefault(x => x.Id == id);
            return View(mazo);
        }
        [HttpPost("Usuario/Usuario/EliminarMazo/")]
        public IActionResult EliminarMazo(Models.Mazo mazo)
        {                 
            var mazodb = Context.Mazo.FirstOrDefault(x => x.Id==mazo.Id);
            if (mazodb!=null)
            {
                ModelState.AddModelError("", "El mazo no puede ser eliminado por que no existe o no se encuentra");
                return RedirectToAction("Index");
            }
            if (Context.Nota.Include(x=>x.IdMazoNavigation).Any(x=>x.IdMazo==mazodb.Id))
            {
                ModelState.AddModelError("", "No se puede eliminar un mazo que tiene notas");
                return RedirectToAction("Index");
            }
            Context.Remove(mazodb);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
