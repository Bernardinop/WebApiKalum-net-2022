using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/CuentaPorCobrar")]
    public class CuentaPorCobrarController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CuentaPorCobrarController> Logger;
        private readonly IMapper Mapper;
        public CuentaPorCobrarController(KalumDbContext _DbContext, ILogger<CuentaPorCobrarController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuentaPorCobrarListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando el proceso de consulta de cargos");
            var cuentaPorCobrar = await DbContext.CuentaPorCobrar.Include(cxc => cxc.Cargo).Include(cxc => cxc.Alumno).ToListAsync();
            if (cuentaPorCobrar == null)
            {
                Logger.LogWarning("No existen cuentas por cobrar");
                return new NoContentResult();
            }
            List<CuentaPorCobrarListDTO> cuentas = Mapper.Map<List<CuentaPorCobrarListDTO>>(cuentaPorCobrar);
            Logger.LogInformation("Se ejecutio la petición de forma existosa");
            return Ok(cuentas);
        }

        [HttpGet("{carne}", Name = "GetCuentasPorCobrar")]
        public async Task<ActionResult<CuentaPorCobrarListDTO>> getCuentaPorCobrar(string carne)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el correlativo " + carne);
            var cuentaPorCobrar = await DbContext.CuentaPorCobrar.Include(cxc => cxc.Cargo).Include(cxc => cxc.Alumno).Where(cxc => cxc.Carne == carne).ToListAsync();
            if (cuentaPorCobrar == null)
            {
                Logger.LogWarning("No existe la cuenta con el correlativo" + carne);
                return new NoContentResult();
            }
            List<CuentaPorCobrarListDTO> cuenta = Mapper.Map<List<CuentaPorCobrarListDTO>>(cuentaPorCobrar);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(cuenta);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<CuentaPorCobrarListDTO>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando paginacion cuenta por cobrar");
            var queryable = DbContext.CuentaPorCobrar.Include(cxc => cxc.Alumno).Include(cxc => cxc.Cargo).AsQueryable();
            var paginacion = new HttpResponsePaginacion<CuentaPorCobrar>(queryable, page);
            if (paginacion.Content == null && paginacion.Content.Count == 0)
            {
                Logger.LogWarning("No existen registros para paginar");
                return NoContent();
            }
            else
            {
                Logger.LogInformation("Finalizando proceso de paginacion alumnos");
                return Ok(paginacion);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CuentaPorCobrar>> Post([FromBody] CuentaPorCobrarCreateDTO value)
        {
            Logger.LogDebug("Iniciando proceso de agregar cuenta por cobrar");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(a => a.Carne == value.Carne);
            if (alumno == null)
            {
                Logger.LogInformation($"No existe alumno con carne {value.Carne}");
                return BadRequest();
            }
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == value.CargoId);
            if (cargo == null)
            {
                Logger.LogInformation($"No existe cargo con id {value.CargoId}");
                return BadRequest();
            }
            CuentaPorCobrar nuevo = Mapper.Map<CuentaPorCobrar>(value);
            await DbContext.CuentaPorCobrar.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de agregar registro a cuenta por cobrar");
            return new CreatedAtRouteResult("GetCuentaxCobrar", new { carne = nuevo.Carne }, value);
        }
    }
}