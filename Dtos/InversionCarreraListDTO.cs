namespace WebApiKalum.Dtos
{
    public class InversionCarreraListDTO
    {
        public string InversionId { get; set; }
        public string CarreraId { get; set; }
        public decimal MontoInscripcion { get; set; }
        public int NumeroPagos { get; set; }
        public decimal MontoPagos { get; set; }
        public InversionCarrera_CarreraTecnicaListDTO CarreraTecnica { get; set; }
    }
}