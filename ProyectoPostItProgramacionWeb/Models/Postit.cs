using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoPostItProgramacionWeb.Models
{
    public partial class Postit
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public int IdUsuario { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
