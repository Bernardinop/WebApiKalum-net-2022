namespace WebApiKalum.Dtos
{
    public class CarreraTecnicaListDTO
    {
        public string CarreraId { get; set; }
        public string Nombre { get; set; }
        public List<AspiranteCarreraListDTO> Aspirante { get; set; }
        public List<InscripcionListDTO> Inscripcion { get; set; }
    }
}