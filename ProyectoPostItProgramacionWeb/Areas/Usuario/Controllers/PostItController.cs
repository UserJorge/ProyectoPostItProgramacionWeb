﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoPostItProgramacionWeb.Models;
using ProyectoPostItProgramacionWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoPostItProgramacionWeb.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize]
    public class PostItController : Controller
    {
        public postitdbContext Context { get; }
        public IWebHostEnvironment Host { get; }

        public PostItController(postitdbContext context, IWebHostEnvironment host)
        {
            Context = context;
            Host = host;
        }
        //Aquí pedirá autorización para visualizar las notas del usuario
        [HttpGet("Usuario/Home/Index/")]
        [HttpGet("Usuario/Home/")]
        [HttpGet("Usuario/")]
        [Authorize]            
        public IActionResult Index()
        {
            var notas =(IEnumerable<Nota>) Context.Nota.Include(x => x.IdMazoNavigation).ThenInclude(x => x.IdUsuarioNavigation).Select(x => x).Where(x => x.IdMazoNavigation.IdUsuarioNavigation.Nombre == User.Identity.Name).ToList();
            return View(notas);
        }
        [Route("Usuario/PostIt/Agregar/")]
        public IActionResult AgregarPostIt()
        {
            PostItViewModel vm = new();
            vm.Nota = new();
            vm.Mazos = Context.Mazo;
            return View(vm);
        }
        [HttpPost]
        public IActionResult AgregarPostIt(PostItViewModel vm)
        {
            vm.Nota.FechaCreacion = DateTime.Now;
            //vm.Nota.IdMazoNavigation = Context.Mazo.FirstOrDefault(x => x.Id == 1);
            if (Context.Mazo.Any(x => x.Id == vm.Nota.IdMazo && x.IdUsuarioNavigation.Nombre==User.Identity.Name))
            {
                ModelState.AddModelError("", "No se puede registrar una nota que ya existe (Usuario)");
                View(vm);
            }
            if (Context.Nota.Any(x => x.Titulo == vm.Nota.Titulo))
            {
                ModelState.AddModelError("", "No se puede registrar una nota que ya existe (título)");
                View(vm);
            }
            if (vm.Nota.Titulo.Length > 50)
            {
                ModelState.AddModelError("", "El título es demasiado grande");
                View(vm);
            }
            if (vm.Nota.Descripcion.Length > 720)
            {
                ModelState.AddModelError("", "Texto en la descripción demasiado grande");
                View(vm);
            }
            Context.Add(vm.Nota);
            Context.SaveChanges();
            if (vm.Audio != null)
            {
                if (!(vm.Audio.ContentType != "audio/mp3" || vm.Audio.ContentType != "audio/mpeg"))
                {
                    ModelState.AddModelError("", "El archivo no está en el formato M4A");
                    View(vm);
                }
                if (vm.Audio.Length > 1024 * 1024 * 10)
                {
                    ModelState.AddModelError("", "El archivo no debe ser mayor a 10MB");
                    View(vm);
                }
                string path = Host.WebRootPath + "/audios/" + $"{vm.Nota.Id}_Audio.mp3";
                FileStream fs = new FileStream(path, FileMode.Create);
                vm.Audio.CopyTo(fs);
            }
            return RedirectToAction("Index");
        }
        [Route("Usuario/PostIt/Editar/{id}")]
        public IActionResult EditarPostIt(string id)
        {
            id = id.Replace("-", " ");
            var nota = Context.Nota.FirstOrDefault(x => x.Titulo == id);
            PostItViewModel vm = new();
            vm.Nota = nota;
            vm.Mazos = Context.Mazo;
            return View(vm);
        }
        [HttpPost("Usuario/PostIt/Editar/")]
        public IActionResult EditarPostIt(PostItViewModel vm)
        {
            var nota = Context.Nota.FirstOrDefault(x => x.Titulo == vm.Nota.Titulo);
            if (vm.Audio != null)
            {
                if (!(vm.Audio.ContentType != "audio/mp3" || vm.Audio.ContentType != "audio/mpeg"))
                {
                    ModelState.AddModelError("", "El archivo no está en el formato M4A");
                    View(vm);
                }
                if (vm.Audio.Length > 1024 * 1024 * 10)
                {
                    ModelState.AddModelError("", "El archivo no debe ser mayor a 10MB");
                    View(vm);
                }
                string path = Host.WebRootPath + "/audios/" + $"{nota.Id}_Audio.mp3";
                FileStream fs = new FileStream(path, FileMode.Create);
                vm.Audio.CopyTo(fs);
            }
            if (nota != null)
            {
                if (nota.Descripcion.Length > 720)
                {
                    ModelState.AddModelError("", "La descripción es demasiado grande");
                    View(vm);
                }
                if (nota.Descripcion.Length == 0)
                {
                    ModelState.AddModelError("", "La descripción es demasiado pequeña");
                    View(vm);
                }
                nota.Descripcion = vm.Nota.Descripcion;
                Context.Update(nota);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "La nota no fue encontrada");
            return View(vm);
        }
        [Route("Usuario/PostIt/Eliminar/{id}")]
        public IActionResult EliminarPostIt(string id)
        {
            var nota = Context.Nota.FirstOrDefault(x => x.Titulo == id.Replace("-", " "));
            if (nota == null)
            {
                ModelState.AddModelError("", "La nota no se encontró o el título no coincide con los registros");
                return RedirectToAction("Index");
            }
            PostItViewModel vm = new();
            vm.Nota = nota;

            return View(vm);
        }
        [HttpPost("Usuario/PostIt/Eliminar/")]
        public IActionResult EliminarPostIt(PostItViewModel vm)
        {
            var nota = Context.Nota.FirstOrDefault(x => x.Titulo == vm.Nota.Titulo);
            if (nota != null)
            {
                if (System.IO.File.Exists(Host.WebRootPath + "/audios/" + $"{nota.Id}_Audio.mp3"))
                {
                    string path = Host.WebRootPath + "/audios/" + $"{nota.Id}_Audio.mp3";
                    System.IO.File.Delete(path);
                }
                Context.Remove(nota);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "No se eliminó, no se encontró la nota");
            return View(vm);
        }
    }
}
