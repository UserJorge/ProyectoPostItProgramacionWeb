using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoPostItProgramacionWeb.Models
{
    public partial class Nota
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdMazo { get; set; }

        public virtual Mazo IdMazoNavigation { get; set; }
    }
}
