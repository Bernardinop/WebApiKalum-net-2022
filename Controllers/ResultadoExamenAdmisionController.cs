using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using AutoMapper;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/ResultadosAdmision")]
    public class ResultadoExamenAdmisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ResultadoExamenAdmisionController> Logger;
        private readonly IMapper Mapper;

        public ResultadoExamenAdmisionController(KalumDbContext _DbContext, ILogger<ResultadoExamenAdmisionController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResultadoExamenAdmisionListDTO>>> Get()
        {
            List<ResultadoExamenAdmision> resultados = null;
            Logger.LogDebug("Iniciando proceso de consulta de resultado del examen de admision");
            resultados = await DbContext.ResultadoExamenAdmision.Include(rea => rea.Aspirante).ToListAsync();
            if (resultados == null || resultados.Count == 0)
            {
                Logger.LogWarning("No existen resultados");
                return new NoContentResult();
            }
            List<ResultadoExamenAdmisionListDTO> resultadosExamenes = Mapper.Map<List<ResultadoExamenAdmisionListDTO>>(resultados);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(resultadosExamenes);
        }

        [HttpGet("{noExpediente}", Name = "GetResultado")]
        public async Task<ActionResult<ResultadoExamenAdmisionListDTO>> GetResultado(string noExpediente)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda de resultado con el expediente " + noExpediente);
            var resultado = await DbContext.ResultadoExamenAdmision.Include(rea => rea.Aspirante).FirstOrDefaultAsync(rea => rea.NoExpediente == noExpediente);
            if (resultado == null)
            {
                Logger.LogWarning("Mo existe resultados para el expediente " + noExpediente);
                return new NoContentResult();
            }
            ResultadoExamenAdmisionListDTO resultadoExamen = Mapper.Map<ResultadoExamenAdmisionListDTO>(resultado);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(resultadoExamen);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<ResultadoExamenAdmisionListDTO>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando paginacion resultados de exmanes de admisión");
            var queryable = DbContext.ResultadoExamenAdmision.Include(re => re.Aspirante).AsQueryable();
            var paginacion = new HttpResponsePaginacion<ResultadoExamenAdmision>(queryable, page);
            if (paginacion.Content == null && paginacion.Content.Count == 0)
            {
                Logger.LogWarning("No existen regisros para paginar");
                return NoContent();
            }
            Logger.LogInformation("Finalizando proceso de paginación de resultados de examen de admisión");
            return Ok(paginacion);
        }

        [HttpPost]
        public async Task<ActionResult<ResultadoExamenAdmision>> Post([FromBody] ResultadoExamenAdmision value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar un resultado de admision nuevo");
            await DbContext.ResultadoExamenAdmision.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso para agregar un resultado de admision nuevo");
            return new CreatedAtRouteResult("GetResultado", new { noExpediente = value.NoExpediente }, value);
        }

        [HttpDelete("{noExpediente}")]
        public async Task<ActionResult<ResultadoExamenAdmision>> Delete(string noExpediente)
        {
            ResultadoExamenAdmision resultados = await DbContext.ResultadoExamenAdmision.FirstOrDefaultAsync(rea => rea.NoExpediente == noExpediente);
            if (resultados == null)
            {
                Logger.LogWarning($"No se encontro ningun resultado con el expediente {noExpediente}");
                return NotFound();
            }
            else
            {
                DbContext.ResultadoExamenAdmision.Remove(resultados);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente el resultado de admision del expediente {noExpediente}");
                return resultados;
            }
        }

        [HttpPut("{noExpediente}")]
        public async Task<ActionResult> Put(string noExpediente, [FromBody] ResultadoExamenAdmision value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion del resultado de admision del expediente {noExpediente}");
            ResultadoExamenAdmision resultados = await DbContext.ResultadoExamenAdmision.FirstOrDefaultAsync(rea => rea.NoExpediente == noExpediente);
            if (resultados == null)
            {
                Logger.LogWarning($"No existe el resultado de admision con el expediente {noExpediente}");
                return BadRequest();
            }
            resultados.NoExpediente = value.NoExpediente;
            resultados.Anio = value.Anio;
            resultados.Descripcion = value.Descripcion;
            resultados.Nota = value.Nota;
            DbContext.Entry(resultados).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Los datos del resultados de amision con el expediente {noExpediente} han sido actualizados correctamente");
            return NoContent();
        }


    }
}