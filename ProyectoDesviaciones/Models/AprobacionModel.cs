using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoDesviaciones.Models
{
    public class AprobacionModel
    {
        public int IdPersonalInvolucrado { get; set; }
        public int IdPersonal { get; set; }
        public int IdDesviaciones { get; set; }
        public System.DateTime FechaAprobacion { get; set; }
        public bool EstadoPersonalInvolucrado { get; set; }
    }
}