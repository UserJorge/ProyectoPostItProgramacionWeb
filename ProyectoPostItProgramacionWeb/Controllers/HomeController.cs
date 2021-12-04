using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoPostItProgramacionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
