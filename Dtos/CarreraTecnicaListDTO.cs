namespace WebApiKalum.Dtos
{
    public class CarreraTecnicaListDTO
    {
        public string CarreraId { get; set; }
        public string Nombre { get; set; }
        public AspiranteListDTO Aspirante { get; set; }
        public InscripcionCreateDTO Inscripcion { get; set; }
    }
}