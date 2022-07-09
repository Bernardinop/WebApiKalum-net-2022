namespace WebApiKalum.Dtos
{
    public class InscripcionListDTO
    {
        public string InscripcionId { get; set; }
        public string Carne { get; set; }
        public string Ciclo { get; set; }
        public string CarreraId { get; set; }
        public string JornadaId { get; set; }
        public DateTime FechaInscripcion { get; set; }
    }
}