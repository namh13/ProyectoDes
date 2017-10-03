using ProyectoDesviaciones.BD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoDesviaciones.Models
{
    public class DesviacionModel
    {
        public int IdDesviacion { get; set; }
        public int IdComponente { get; set; }
        public string NumeroDeParte { get; set; } // Numero de Parte
        public int IdCliente { get; set; }
        public string Cliente { get; set; } // Cliente
        public string NumeroDesviacion { get; set; }
        public System.DateTime FechaDesviacion { get; set; }
        public string RequeridoPor { get; set; }
        public string DescripcionProcesoCorrecto { get; set; }
        public string DescripcionCondicionComponente { get; set; }
        public string DescripcionRazonDesviacion { get; set; }
        public string DescripcionPlanDeAccion { get; set; }
        public System.DateTime DescripcionFechaDesviacion { get; set; }
        public string DescripcionResponsable { get; set; }
        public bool CriterioAceptado { get; set; }
        public string DetalleCriterioAceptado { get; set; }
        public string Vencimiento { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public Nullable<int> CantidadDePiezas { get; set; }
        public int IdTipoDesviacion { get; set; } 
        public string TipoDesviacion { get; set; } //TipoDesviacion
        public List<int> ListaAreas { get; set; }
        public List<int> ListaDocumentos { get; set; }
        
        public List<string> LstAreas { get; set; }
        public List<string> LstDocumentos { get; set; }

        public List<ListaAreasDeshabilitar> Areas { get; set; }
        public List<ListaDocumentosDeshabilitar> Documentos { get; set; }

        public int[] SelectListAreas { get; set; }
        public int[] SelectListDocumentos { get; set; }
    }
}