using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class Alumno
    {
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [StringLength(12, MinimumLength =7, ErrorMessage = "la cantidad minima de caracteres es de {2} y maxima es {1} para el campo {0}")]
        public string Carne { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [StringLength(128, MinimumLength =5, ErrorMessage = "la cantidad minima de caracteres es de {2} y maxima es {1} para el campo {0}")]
        public string Apellidos { get; set;}
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [StringLength(128, MinimumLength =5, ErrorMessage = "la cantidad minima de caracteres es de {2} y maxima es {1} para el campo {0}")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [StringLength(128, MinimumLength =8, ErrorMessage = "la cantidad minima de caracteres es de {2} y maxima es {1} para el campo {0}")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [StringLength(20, MinimumLength =8, ErrorMessage = "la cantidad minima de caracteres es de {2} y maxima es {1} para el campo {0}")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [StringLength(128, MinimumLength =15, ErrorMessage = "la cantidad minima de caracteres es de {2} y maxima es {1} para el campo {0}")]
        public string Email { get; set; }
        public virtual List <Inscripcion> Inscripciones { get; set; }
        public virtual List<CuentaPorCobrar> CuentaPorCobrar { get; set; }
    }
}