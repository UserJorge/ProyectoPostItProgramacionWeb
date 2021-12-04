using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoPostItProgramacionWeb.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Postit = new HashSet<Postit>();
        }

        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string ClaveAcceso { get; set; }

        public virtual ICollection<Postit> Postit { get; set; }
    }
}
