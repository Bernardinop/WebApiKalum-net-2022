namespace WebApiKalum.Entities
{
    public class CuentaPorCobrar
    {
        public string Correlativo { get; set;}
        public string Anio { get; set;}
        public string Carne { get; set; }
        public string CargoId { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCargo { get; set; }
        public DateTime FechaAplica { get; set; }
        public decimal Monto { get; set; }
        public decimal Mora { get; set; }
        public decimal Descuento { get; set; }
        public virtual Cargo Cargo { get; set; }       
        public virtual Alumno Alumno { get; set; }
    }
}