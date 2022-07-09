using System.ComponentModel.DataAnnotations;
namespace WebApiKalum.Dtos
{
    public class CargoCreateDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Prefijo { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Currency)]
        public decimal Monto { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public bool GeneraMora { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression(@"^\d+$")]
        public int PorcentajeMora { get; set; } 
    }
}