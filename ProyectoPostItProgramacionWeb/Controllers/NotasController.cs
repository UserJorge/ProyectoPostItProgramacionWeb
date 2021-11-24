using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoPostItProgramacionWeb.Controllers
{
    public class NotasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
