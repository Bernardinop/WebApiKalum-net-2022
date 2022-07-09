using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Jornada")]
    public class JornadaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<JornadaController> Logger;
        private readonly IMapper Mapper;
        public JornadaController(KalumDbContext _DbContext, ILogger<JornadaController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JornadaListDTO>>> Get()
        {
            List<Jornada> jornadas = null;
            Logger.LogDebug("Iniciando proceso consulta jornadas");
            jornadas = await DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).ToListAsync();
            if (jornadas == null || jornadas.Count == 0)
            {
                Logger.LogWarning("No existen jornadas");
                return new NoContentResult();
            }
            List<JornadaListDTO> horarios = Mapper.Map<List<JornadaListDTO>>(jornadas);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(horarios);
        }

        [HttpGet("{id}", Name = "GetJornada")]
        public async Task<ActionResult<JornadaListDTO>> GetJornada(string id)
        {
            Logger.LogDebug("Iniciando proceso de busqueda con el id " + id);
            var jornada = await DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).FirstOrDefaultAsync(j => j.JornadaId == id);
            if (jornada == null)
            {
                Logger.LogWarning("No existe la jornada con el id " + id);
                return new NoContentResult();
            }
            JornadaListDTO horario = Mapper.Map<JornadaListDTO>(jornada);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(horario);
        }

        [HttpPost]
        public async Task<ActionResult<Jornada>> Post([FromBody] Jornada value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar una nueva jornada");
            value.JornadaId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Jornada.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso para agregar una nueva jornada");
            return new CreatedAtRouteResult("GetJornada", new { id = value.JornadaId }, value);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<JornadaListDTO>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando paginacion jornada");
            var queryable = DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).AsQueryable();
            var paginacion = new HttpResponsePaginacion<Jornada>(queryable, page);
            if (paginacion.Content == null && paginacion.Content.Count == 0)
            {
                Logger.LogWarning("No existen registros para paginar");
                return NoContent();
            }
            List<JornadaListDTO> jornadas = Mapper.Map<List<JornadaListDTO>>(paginacion.Content);
            Logger.LogInformation("Finalizando proceso de paginacion jornada");
            return Ok(paginacion);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Jornada>> Delete(string id)
        {
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if (jornada == null)
            {
                Logger.LogWarning($"No se encontro ninguna Jornada con el ID {id}");
                return NotFound();
            }
            else
            {
                DbContext.Jornada.Remove(jornada);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente la jornada con el ID {id}");
                return jornada;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Jornada value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de la Jornada con el ID {id}");
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if (jornada == null)
            {
                Logger.LogWarning($"No existe la Jornada con el ID {id}");
                return BadRequest();
            }
            jornada.NombreCorto = value.NombreCorto;
            jornada.Descripcion = value.Descripcion;
            DbContext.Entry(jornada).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Los datos de la jornada con el ID {id} han sido actualizados correctamente");
            return NoContent();
        }
    }
}