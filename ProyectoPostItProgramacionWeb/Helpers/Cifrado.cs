using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPostItProgramacionWeb.Helpers
{
    public static class Cifrado
    {
        public static string GetHash(string cadena)
        {
            var algoritmo = SHA512.Create();
            var arreglo = Encoding.UTF8.GetBytes(cadena+"str0ngP4ssW0rd");
            var hash = algoritmo.ComputeHash(arreglo).Select(x => x.ToString("x2"));
            return string.Join("", hash);
        }
    }
}
