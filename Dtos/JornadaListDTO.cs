using System.ComponentModel.DataAnnotations;
namespace WebApiKalum.Dtos
{
    public class JornadaListDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string JornadaId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NombreCorto { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Descripcion { get; set; }
        public virtual List<AspiranteListDTO> Aspirantes { get; set; }
        public virtual List<InscripcionListDTO> Inscripciones { get; set; }
    }
}