namespace WebApiKalum.Dtos
{
    public class InscripcionPagoListDTO
    {
        public string BoletaPago { get; set; }
        public string NoExpediente { get; set; }
        public string Anio { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public  AspiranteCarreraListDTO Aspirante { get; set; }   
    }
}