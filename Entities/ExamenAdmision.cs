using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Entities
{
    public class ExamenAdmision
    {
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        public string ExamenId { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        public DateTime FechaExamen { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; }
    }
}