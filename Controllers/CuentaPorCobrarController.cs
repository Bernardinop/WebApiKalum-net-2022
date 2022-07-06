using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/CuentaxCobrar")]
    public class CuentaPorCobrarController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CuentaPorCobrarController> Logger;
        public CuentaPorCobrarController(KalumDbContext _DbContext, ILogger<CuentaPorCobrarController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuentaPorCobrar>>> Get()
        {
            Logger.LogDebug("Iniciando el proceso de consulta de cargos");
            var cuentaPorCobrar = await DbContext.CuentaPorCobrar.Include(cxc => cxc.Cargo).Include(cxc => cxc.Alumno).ToListAsync();
            if (cuentaPorCobrar == null)
            {
                Logger.LogWarning("No existen cuentas por cobrar");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecutio la petición de forma existosa");
            return Ok(cuentaPorCobrar);
        }

        [HttpGet("{carne}", Name = "GetCuentasPorCobrar")]
        public async Task<ActionResult<CuentaPorCobrar>> getCuentaPorCobrar(string carne)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el correlativo " + carne);
            var cuentaPorCobrar = await DbContext.CuentaPorCobrar.Include(cxc => cxc.Cargo).Include(cxc => cxc.Alumno).Where(cxc => cxc.Carne == carne).ToListAsync();
            if (cuentaPorCobrar == null)
            {
                Logger.LogWarning("No existe la cuenta con el correlativo" + carne);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(cuentaPorCobrar);
        }

        [HttpPost]
        public async Task<ActionResult<CuentaPorCobrar>> Post([FromBody] CuentaPorCobrar value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar una nueva cuenta por cobrar");
            await DbContext.CuentaPorCobrar.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso para agregar una nueva cuenta por cobrar");
            return new CreatedAtRouteResult("GetCuentaPorCobrar", new { correlativo = value.Correlativo }, value);
        }
    }
}