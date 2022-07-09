using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class ExamenAdmisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ExamenAdmisionController> Logger;
        private readonly IMapper Mapper;

        public ExamenAdmisionController(KalumDbContext dbContext, ILogger<ExamenAdmisionController> _Logger, IMapper _Mapper)
        {
            this.DbContext = dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamenAdmisionListDTO>>> Get()
        {
            List<ExamenAdmision> examenes = null;
            Logger.LogDebug("Iniciando proceso de consulta examenes admisión");
            examenes = await DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).ToListAsync();
            if (examenes == null || examenes.Count == 0)
            {
                Logger.LogWarning("No existen examenes de admisión");
                return new NoContentResult();
            }
            List<ExamenAdmisionListDTO> examenesAdmision = Mapper.Map<List<ExamenAdmisionListDTO>>(examenes);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(examenesAdmision);
        }

        [HttpGet("{id}", Name = "GetExamenAdmision")]
        public async Task<ActionResult<ExamenAdmisionListDTO>> GetExamenAdmision(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + id);
            var examen = await DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).FirstOrDefaultAsync(ea => ea.ExamenId == id);
            if (examen == null)
            {
                Logger.LogWarning("No existe el examen de admisión con el id " + id);
                return new NoContentResult();
            }
            ExamenAdmisionListDTO examenAdmision = Mapper.Map<ExamenAdmisionListDTO>(examen);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(examenAdmision);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<ExamenAdmisionListDTO>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando paginacion examen admision");
            var queryable = DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).AsQueryable();
            var paginacion = new HttpResponsePaginacion<ExamenAdmision>(queryable, page);
            if (paginacion.Content == null && paginacion.Content.Count == 0)
            {
                Logger.LogWarning("No existen registros para paginar");
                return NoContent();
            }
            else
            {
                Logger.LogInformation("Finalizando proceso de paginacion de examen de admision");
                return Ok(paginacion);
            }

        }

        [HttpPost]
        public async Task<ActionResult<ExamenAdmision>> Post([FromBody] ExamenAdmision value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar un Examen de admision nuevo");
            value.ExamenId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.ExamenAdmision.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso para agregar un examen de admision nuevo");
            return new CreatedAtRouteResult("GetExamenAdmision", new { id = value.ExamenId }, value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ExamenAdmision>> Delete(string id)
        {
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ex => ex.ExamenId == id);
            if (examenAdmision == null)
            {
                Logger.LogWarning($"No se encontro ningun examen de admision con el ID {id}");
                return NotFound();
            }
            else
            {
                DbContext.ExamenAdmision.Remove(examenAdmision);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente el examen de admision con el ID {id}");
                return examenAdmision;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] ExamenAdmision value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion del examen de admision con el ID {id}");
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ex => ex.ExamenId == id);
            if (examenAdmision == null)
            {
                Logger.LogWarning($"No existe el examen de admision con ID {id}");
                return BadRequest();
            }
            examenAdmision.FechaExamen = value.FechaExamen;
            DbContext.Entry(examenAdmision).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Los datos del examen de admision con el ID {id} han sido actualizados correctamente");
            return NoContent();
        }
    }
}