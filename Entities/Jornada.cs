using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class Jornada
    {
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        public string JornadaId { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [StringLength(5, MinimumLength =2, ErrorMessage = "la cantidad minima de caracteres es de {2} y maxima es {1} para el campo {0}")]
        public string NombreCorto { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [StringLength(30, MinimumLength =10, ErrorMessage = "la cantidad minima de caracteres es de {2} y maxima es {1} para el campo {0}")]
        public string Descripcion { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
    }
}