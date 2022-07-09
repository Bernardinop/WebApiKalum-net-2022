using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using WebApiKalum.Helpers;

namespace WebApiKalum.Entities
{
    public class Aspirante //: IValidatableObject
    {
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        [StringLength(12, MinimumLength = 11, ErrorMessage = "la cantidad minima de caracteres es de {2} y maxima es {1} para el campo {0}")]
        [NoExpediente]
        public string NoExpediente { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "El Campo {0} es Requerido")]
        public string Telefono { get; set; }
        [EmailAddress(ErrorMessage = "El correo electronico no es valido")]
        public string Email { get; set; }
        public string Estatus { get; set; } = "No Asignado";
        public string CarreraId { get; set; }
        public string JornadaId { get; set; }
        public string ExamenId { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }
        public virtual Jornada Jornada { get; set; }
        public virtual ExamenAdmision ExamenAdmision { get; set; }
        public virtual List<InscripcionPago> InscripcionPagos { get; set; }
        public virtual List<ResultadoExamenAdmision> ResultadosExamenAdmision { get; set; }
        /*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //bool expedienteValid = false;
            if (!string.IsNullOrEmpty(NoExpediente))
            {
                if (!NoExpediente.Contains("-"))
                {
                    yield return new ValidationResult("El numero de expediente es invalido, no contiene un '-'", new string[] { nameof(NoExpediente) });
                }
                else
                {
                    int guion = NoExpediente.IndexOf("-");
                    string exp = NoExpediente.Substring(0, guion);
                    string numero = NoExpediente.Substring(guion + 1, NoExpediente.Length - 4);
                    if (!exp.ToUpper().Equals("EXP") || !Information.IsNumeric(numero))
                    {
                        yield return new ValidationResult("El numero del expediente no contiene la nomenglatura adecuada", new string[] { nameof(NoExpediente) });
                    }
                }
            }
        }*/
    }
}