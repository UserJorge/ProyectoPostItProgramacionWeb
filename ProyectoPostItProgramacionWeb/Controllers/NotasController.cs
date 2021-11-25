using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoPostItProgramacionWeb.Controllers
{
    public class NotasController : Controller
    {
        [HttpGet("Notas/")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("Notas/Agregar/")]
        public IActionResult AgregarNota()
        {
            return View();
        }
        [HttpGet("Notas/Editar/")]
        public IActionResult EditarNota()
        {
            return View();
        }
        [HttpGet("Notas/Eliminar/")]
        public IActionResult EliminarNota()
        {
            return View();
        }
    }
}

