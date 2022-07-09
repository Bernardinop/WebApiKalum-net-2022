using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApiKalum.Dtos;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Alumno")]
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AlumnoController> Logger;
        private readonly IMapper Mapper;

        public AlumnoController(KalumDbContext _DbContext, ILogger<AlumnoController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlumnoListDTO>>> Get()
        {
            List<Alumno> alumnos = null;
            Logger.LogDebug("Iniciando proceso de consulta de alumnos");
            alumnos = await DbContext.Alumno.Include(a => a.Inscripciones).Include(a => a.CuentaPorCobrar).ToListAsync();
            if (alumnos == null || alumnos.Count == 0)
            {
                Logger.LogWarning("No existen alumnos");
                return new NoContentResult();
            }
            List<AlumnoListDTO> estudiantes = Mapper.Map<List<AlumnoListDTO>>(alumnos);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(estudiantes);
        }

        [HttpGet("{carne}", Name = "GetAlumno")]
        public async Task<ActionResult<AlumnoListDTO>> GetAlumno(string carne)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + carne);
            var alumno = await DbContext.Alumno.Include(a => a.Inscripciones).Include(a => a.CuentaPorCobrar).FirstOrDefaultAsync(a => a.Carne == carne);
            if (alumno == null)
            {
                Logger.LogWarning("No existe el alumno con el carne " + carne);
                return new NoContentResult();
            }
            AlumnoListDTO estudiante = Mapper.Map<AlumnoListDTO>(alumno);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(estudiante);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<AlumnoListDTO>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando paginacion de alumno");
            var queryable = DbContext.Alumno.Include(a => a.Inscripciones).Include(a => a.CuentaPorCobrar).AsQueryable();
            var paginacion = new HttpResponsePaginacion<Alumno>(queryable, page);
            if(paginacion.Content == null && paginacion.Content.Count == 0) 
            {
                Logger.LogWarning("No existen registros para paginar");
                return NoContent();
            }
            else
            {
                return Ok(paginacion);
            }            
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
            Logger.LogDebug("Iniciando proceso de eliminación");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(a => a.Carne == carne);
            CuentaPorCobrar cuentaPorCobrar = await DbContext.CuentaPorCobrar.FirstOrDefaultAsync(cxc => cxc.Carne == carne);
            if (alumno == null)
            {
                Logger.LogWarning($"No se encontro ningun Alumno con el carne {carne}");
                return NotFound();
            }
            else if (cuentaPorCobrar != null)
            {
                Logger.LogWarning($"No se puede eliminar el alumno con carne {carne} porque posee cuenta por cobrar");
                return BadRequest();
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