using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/InversionCarrera")]
    public class InversionCarreraTecnicaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<InversionCarreraTecnicaController> Logger;
        private readonly IMapper Mapper;

        public InversionCarreraTecnicaController(KalumDbContext _DbContext, ILogger<InversionCarreraTecnicaController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InversionCarreraListDTO>>> Get()
        {
            List<InversionCarreraTecnica> inversionesCarreras = null;
            Logger.LogDebug("Iniciando consulta de inversione carrera tecnica");
            inversionesCarreras = await DbContext.InversionCarreraTecnica.Include(ict => ict.CarreraTecnica).ToListAsync();
            if (inversionesCarreras == null)
            {
                Logger.LogWarning("No existen inversiones de carreras tecnicas");
                return new NoContentResult();
            }
            else
            {
                List<InversionCarreraListDTO> inversiones = Mapper.Map<List<InversionCarreraListDTO>>(inversionesCarreras);
                Logger.LogInformation("Se ejecutó la petición de forma exitosa");
                return Ok(inversiones);
            }
        }

        [HttpGet("{id}", Name = "GetInversion")]
        public async Task<ActionResult<InversionCarreraListDTO>> GetInversion(string id)
        {
            Logger.LogDebug($"Iniciando proceso de busqueda con el id {id}");
            var inversionCarrera = await DbContext.InversionCarreraTecnica.Include(ict => ict.CarreraTecnica).FirstOrDefaultAsync(ict => ict.InversionId == id);
            if (inversionCarrera == null)
            {
                Logger.LogWarning($"No existe la inversion con el id {id}");
                return new NoContentResult();
            }
            else
            {
                InversionCarreraListDTO inversion = Mapper.Map<InversionCarreraListDTO>(inversionCarrera);
                Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
                return Ok(inversion);
            }
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<InversionCarreraListDTO>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando paginacion inversion carrera tecnica");
            var queryable = DbContext.InversionCarreraTecnica.Include(ict => ict.CarreraTecnica).AsQueryable();
            var paginacion = new HttpResponsePaginacion<InversionCarreraTecnica>(queryable, page);
            if (paginacion.Content == null && paginacion.Content.Count == 0)
            {
                Logger.LogWarning("No existen registros para paginar");
                return NoContent();
            }
            else
            {
                Logger.LogInformation("Finalizando proceso de paginacion inversion carrera tecnica");
                return Ok(paginacion);
            }
        }

        [HttpPost]
        public async Task<ActionResult<InversionCarreraTecnica>> Post([FromBody] InversionCarreraCreateDTO value)
        {
            Logger.LogDebug("Iniciando proceso de agregar una nueva inversion");
            CarreraTecnica carrera = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            InversionCarreraTecnica nuevo = Mapper.Map<InversionCarreraTecnica>(value);
            nuevo.InversionId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.InversionCarreraTecnica.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de agregar una inversion");
            return new CreatedAtRouteResult("GetInversion", new { id = nuevo.InversionId }, value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<InversionCarreraTecnica>> Delete(string id)
        {
            Logger.LogDebug("Iniciando proceso de eliminación");
            InversionCarreraTecnica inversion = await DbContext.InversionCarreraTecnica.FirstOrDefaultAsync(ict => ict.InversionId == id);
            if (inversion == null)
            {
                Logger.LogWarning($"No se encontro ninguna inversion con el id {id}");
                return NotFound();
            }
            else
            {
                DbContext.InversionCarreraTecnica.Remove(inversion);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente la inversion con el id {id}");
                return inversion;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] InversionCarreraTecnica value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualización de la inversion con id {id}");
            InversionCarreraTecnica inversion = await DbContext.InversionCarreraTecnica.FirstOrDefaultAsync(ict => ict.InversionId == id);
            if (inversion == null)
            {
                Logger.LogWarning($"No se encontro ninguna inversion con el id {id}");
                return NotFound();
            }
            else
            {
                inversion.MontoInscripcion = value.MontoInscripcion;
                inversion.NumeroPagos = value.NumeroPagos;
                inversion.MontoPagos = value.MontoPagos;
                DbContext.Entry(inversion).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
                Logger.LogInformation("Los datos han sido actualizados correctamente");
                return NoContent();
            }
        }

    }
}