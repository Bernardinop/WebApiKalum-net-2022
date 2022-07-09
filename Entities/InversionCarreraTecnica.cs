using System.ComponentModel.DataAnnotations;
namespace WebApiKalum.Entities
{
    public class InversionCarreraTecnica
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string InversionId { get; set; }
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
        public virtual CarreraTecnica CarreraTecnica { get; set; }
    }
}