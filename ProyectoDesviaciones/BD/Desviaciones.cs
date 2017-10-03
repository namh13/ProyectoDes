//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoDesviaciones.BD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Desviaciones
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Desviaciones()
        {
            this.ResultadoDeAreaInvolucrada = new HashSet<ResultadoDeAreaInvolucrada>();
            this.ResultadoDeDocumentosRelacionados = new HashSet<ResultadoDeDocumentosRelacionados>();
            this.ResultadoDePersonalInvolucrado = new HashSet<ResultadoDePersonalInvolucrado>();
        }
    
        public int IdDesviacion { get; set; }
        public int IdComponente { get; set; }
        public int IdCliente { get; set; }
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
        public Nullable<System.DateTime> FechaVencimiento { get; set; }
        public Nullable<int> CantidadDePiezas { get; set; }
        public int IdTipoDesviacion { get; set; }
        public string CreadoPor { get; set; }
        public bool EstadoDesviacion { get; set; }
    
        public virtual Clientes Clientes { get; set; }
        public virtual Componentes Componentes { get; set; }
        public virtual TipoDesviacion TipoDesviacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResultadoDeAreaInvolucrada> ResultadoDeAreaInvolucrada { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResultadoDeDocumentosRelacionados> ResultadoDeDocumentosRelacionados { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResultadoDePersonalInvolucrado> ResultadoDePersonalInvolucrado { get; set; }
    }
}