using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos
{
    public class ExamenAdmisionCreateDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaExamen { get; set; }
    }
}