using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class Cargo
    {
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        public string CargoId { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [StringLength(128, MinimumLength =5, ErrorMessage = "la cantidad minima de caracteres es de {2} y maxima es {1} para el campo {0}")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [StringLength(5, MinimumLength =3, ErrorMessage = "la cantidad minima de caracteres es de {2} y maxima es {1} para el campo {0}")]
        public string Prefijo { get; set; }
        public decimal Monto { get; set; }
        public bool GeneraMora { get; set; }
        public int PorcentajeMora { get; set; }
        public virtual List<CuentaPorCobrar> CuentaPorCobrar { get; set; }
    }
}