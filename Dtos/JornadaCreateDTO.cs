using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos
{
    public class JornadaCreateDTO
    {
        [StringLength(5, MinimumLength = 2, ErrorMessage = "la cantidad minima de caracteres es de {2} y maxima es {1} para el campo {0}")]
        public string NombreCorto { get; set; }
        [StringLength(30, MinimumLength = 10, ErrorMessage = "la cantidad minima de caracteres es de {2} y maxima es {1} para el campo {0}")]
        public string Descripcion { get; set; }
    }
}