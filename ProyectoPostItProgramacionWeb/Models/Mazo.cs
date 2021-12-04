using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoPostItProgramacionWeb.Models
{
    public partial class Mazo
    {
        public Mazo()
        {
            Nota = new HashSet<Nota>();
        }

        public int Id { get; set; }
        public string Titulo { get; set; }
        public int IdUsuario { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Nota> Nota { get; set; }
    }
}
