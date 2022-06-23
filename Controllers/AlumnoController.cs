using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Alumno")]
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AlumnoController> Logger;

        public AlumnoController(KalumDbContext _DbContext, ILogger<AlumnoController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlumnoController>>> Get()
        {
            List<Alumno> alumnos = null;
            Logger.LogDebug("Iniciando proceso de consulta de alumnos");
            alumnos = await DbContext.Alumno.Include(a => a.Inscripciones).Include(a => a.CuentaPorCobrar).ToListAsync();
            if (alumnos == null || alumnos.Count == 0)
            {
                Logger.LogWarning("No existen alumnos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(alumnos);
        }

        [HttpGet("{carne}", Name = "GetAlumno")]
        public async Task<ActionResult<Alumno>> GetAlumno(string carne)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + carne);
            var alumno = await DbContext.Alumno.Include(a => a.Inscripciones).Include(a => a.CuentaPorCobrar).FirstOrDefaultAsync(a => a.Carne == carne);
            if (alumno == null)
            {
                Logger.LogWarning("No existe el alumno con el carne " + carne);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(alumno);
        }

        [HttpPost]
        public async Task<ActionResult<Alumno>> Post([FromBody] Alumno value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar un alumno nuevo");
            await DbContext.Alumno.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso para agregar un alumno nuevo");
            return new CreatedAtRouteResult("GetAlumno", new { carne = value.Carne }, value);
        }

        [HttpDelete("{carne}")]
        public async Task<ActionResult<Alumno>> Delete(string carne)
        {
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(a => a.Carne == carne);
            if (alumno == null)
            {
                Logger.LogWarning($"No se encontro ningun Alumno con el carne {carne}");
                return NotFound();
            }
            else
            {
                DbContext.Alumno.Remove(alumno);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente el alumno con el carne {carne}");
                return alumno;
            }
        }

        [HttpPut("{carne}")]
        public async Task<ActionResult> Put(string carne, [FromBody] Alumno value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de alumno con el carne {carne}");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(a => a.Carne == carne);
            if (alumno == null)
            {
                Logger.LogWarning($"No existe el alumno con el carne {carne}");
                return BadRequest();
            }
            alumno.Apellidos = value.Apellidos;
            alumno.Nombres = value.Nombres;
            alumno.Direccion = value.Direccion;
            alumno.Telefono = value.Telefono;
            alumno.Email = value.Email;
            DbContext.Entry(alumno).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Los datos del del alumno con el carne {carne} han sido actualizados correctamente");
            return NoContent();
        }
    }
}