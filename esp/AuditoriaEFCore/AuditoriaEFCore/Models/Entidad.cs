using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditoriaEFCore.Models
{
    public abstract class Entidad
    {
        public int Id { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTimeOffset FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTimeOffset FechaModificacion { get; set; }
    }
}
