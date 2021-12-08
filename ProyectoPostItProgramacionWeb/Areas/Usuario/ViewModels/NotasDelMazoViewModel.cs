using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoPostItProgramacionWeb.Areas.Usuario.ViewModels
{
    public class NotasDelMazoViewModel
    {
        public IEnumerable<Models.Nota> Notas { get; set; }
        public IEnumerable<Models.Mazo> Mazos { get; set; }
        public int Mazo { get; set; }
    }
}
