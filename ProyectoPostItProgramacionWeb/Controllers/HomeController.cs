using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoPostItProgramacionWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult RegistrarUsuario()
        {
            return View();
        }
        public IActionResult IniciarSesionUsuario()
        {
            return View();
        }

    }
}
