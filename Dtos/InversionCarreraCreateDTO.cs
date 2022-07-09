using System.ComponentModel.DataAnnotations;
namespace WebApiKalum.Dtos
{
    public class InversionCarreraCreateDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string CarreraId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Currency)]
        public decimal MontoInscripcion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^\d+$")]
        public int NumeroPagos { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Currency)]
        public decimal MontoPagos { get; set; }

    }
}