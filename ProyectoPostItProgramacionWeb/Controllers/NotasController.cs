using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoPostItProgramacionWeb.Models;
using ProyectoPostItProgramacionWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoPostItProgramacionWeb.Controllers
{
    public class NotasController : Controller
    {
        public postitdbContext Context { get; }
        public IWebHostEnvironment Host { get; }

        public NotasController(postitdbContext context, IWebHostEnvironment env)
        {
            Context = context;
            Host = env;
        }
        [HttpGet("Notas/")]
        public IActionResult Index()
        {
            var notas =(IEnumerable<Nota>)Context.Nota.Include(x => x.IdMazoNavigation).ThenInclude(x => x.IdUsuarioNavigation).Select(x => x.IdMazoNavigation.IdUsuario == 1 && x.IdMazo == 1).ToList();
            return View(notas);
        }
        [HttpGet("Notas/Agregar/")]
        public IActionResult AgregarNota()
        {
            PostItViewModel vm = new();
            vm.Nota = new();          
            return View(vm);  
        }
        [HttpPost("Notas/Agregar/")]
        public IActionResult AgregarNota(PostItViewModel vm)
        {
            vm.Nota.FechaCreacion = DateTime.Now;
            vm.Nota.IdMazo = 1;
            //vm.Nota.IdMazoNavigation = Context.Mazo.FirstOrDefault(x => x.Id == 1);
            if (!(vm.Audio.ContentType!= "audio/mp3"|| vm.Audio.ContentType != "audio/mpeg"))
            {
                ModelState.AddModelError("", "El archivo no está en el formato M4A");
                View(vm);
            }
            if (vm.Audio.Length>1024*1024*10)
            {
                ModelState.AddModelError("", "El archivo no debe ser mayor a 10MB");
                View(vm);
            }
            if (Context.Nota.Any(x=>x.Titulo==vm.Nota.Titulo))
            {
                ModelState.AddModelError("", "No se puede registrar una nota que ya existe (título)");
                View(vm);
            }
            if (vm.Nota.Titulo.Length > 50)
            {
                ModelState.AddModelError("", "El título es demasiado grande");
                View(vm);
            }
            if (vm.Nota.Descripcion.Length>720)
            {
                ModelState.AddModelError("", "Texto en la descripción demasiado grande");
                View(vm);
            }
            Context.Add(vm.Nota);
            Context.SaveChanges();
            string path = Host.WebRootPath + "/audios/"+$"{vm.Nota.Id}_Audio.mp3";
            FileStream fs = new FileStream(path, FileMode.Create);
            vm.Audio.CopyTo(fs);         
            return RedirectToAction("Index");
        }
        [HttpGet("Notas/Editar/{id}")]
        public IActionResult EditarNota(string id)
        {
            id = id.Replace("-", " ");
            var nota = Context.Nota.FirstOrDefault(x => x.Titulo == id);
            return View(nota);
        }
        [HttpPost("Notas/Editar/")]
        public IActionResult EditarNota(PostItViewModel vm)
        {          
            var nota = Context.Nota.FirstOrDefault(x => x.Titulo == vm.Nota.Titulo);
            if (nota!=null)
            {
                nota.Descripcion = vm.Nota.Descripcion;
                Context.Update(nota);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "La nota no fue encontrada");
            return View(nota);
        }
        [HttpGet("Notas/Eliminar/{id}")]
        public IActionResult EliminarNota(string id)
        {
            var nota = Context.Nota.FirstOrDefault(x => x.Titulo == id.Replace("-"," "));
            if (nota!=null)
            {
                ModelState.AddModelError("", "La nota no se encontró o el título no coincide con los registros");
                return RedirectToAction("Index");
            }          
            return View(nota);
        }
        [HttpPost("Notas/Eliminar/")]
        public IActionResult EliminarNota(PostItViewModel vm)
        {
            var nota = Context.Nota.FirstOrDefault(x => x.Titulo == vm.Nota.Titulo);
            if (nota!=null)
            {
                Context.Remove(nota);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "No se eliminó, no se encontró la nota");
            return View(vm);
        }
    }
}

