using Microsoft.AspNetCore.Http;
using ProyectoPostItProgramacionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoPostItProgramacionWeb.ViewModels
{
    public class PostItViewModel
    {
        public Nota Nota { get; set; }
        public IFormFile Audio { get; set; }
    }
}
