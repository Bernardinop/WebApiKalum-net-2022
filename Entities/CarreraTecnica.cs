namespace WebApiKalum.Entities
{
    public class CarreraTecnica
    {
        public string CarreraId { get; set; }
        public string Nombre { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
        public virtual List<InversionCarreraTecnica> InversionCarreraTecnica { get; set; }
    }
}