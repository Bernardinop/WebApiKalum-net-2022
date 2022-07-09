namespace WebApiKalum.Dtos
{
    public class ResultadoExamenAdmisionListDTO
    {
        public string NoExpediente { get; set; }
        public string Anio { get; set; }
        public string Descripcion { get; set; }
        public int Nota { get; set; }
        public  AspiranteCarreraListDTO Aspirante { get; set; }
    }
}