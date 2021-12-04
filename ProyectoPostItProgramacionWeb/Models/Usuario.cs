using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoPostItProgramacionWeb.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Mazo = new HashSet<Mazo>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Mazo> Mazo { get; set; }
    }
}
