using Microsoft.AspNetCore.Mvc;
using ProyectoPostItProgramacionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoPostItProgramacionWeb.Areas.Usuario.Controllers
{
    public class PostItController : Controller
    {
        public postitdbContext Context { get; }

        public PostItController(postitdbContext context)
        {
            Context = context;
        }
        
        [Route("Usuario/Index")]
        [Route("Usuario/")]
        [Route("Usuario/Home")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Usuario/Agregar/")]
        public IActionResult AgregarPostIt()
        {
            return View();
        }
        [Route("Usuario/Editar/{id}")]
        public IActionResult EditarPostIt(int id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult EditarPostIt()
        {
            return View();
        }
        [Route("Usuario/Eliminar/{id}")]
        public IActionResult EliminarPostIt(int id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult EliminarPostIt()
        {
            return View();
        }
    }
}
