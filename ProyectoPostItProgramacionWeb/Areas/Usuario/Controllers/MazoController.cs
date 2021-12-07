using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoPostItProgramacionWeb.Areas.Usuario.Controllers
{
    public class MazoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
