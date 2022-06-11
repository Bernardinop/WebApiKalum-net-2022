using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class CarreraTecnica
    {
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        public string CarreraId { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [StringLength(128, MinimumLength =5, ErrorMessage = "la cantidad minima de caracteres es de {2} y maxima es {1} para el campo {0}")]
        public string Nombre { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
        public virtual List<InversionCarreraTecnica> InversionCarreraTecnica { get; set; }
    }
}