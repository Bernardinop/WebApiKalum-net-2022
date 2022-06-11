using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class CargoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CargoController> Logger;

        public CargoController(KalumDbContext _DbContext, ILogger<CargoController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cargo>>> Get()
        { 
            Logger.LogDebug("Iniciando el proceso de consulta de cargos");
            var cargos = await DbContext.Cargo.Include(c => c.CuentaPorCobrar).ToListAsync();
            if(cargos == null)
            {
                Logger.LogWarning("No existen cargos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecutio la petición de forma existosa");
            return Ok(cargos);
        }

        [HttpGet("{id}", Name = "GetCargo")]
        public async Task<ActionResult<Cargo>> GetCargo(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + id);
            var cargo = await DbContext.Cargo.Include(c => c.CuentaPorCobrar).FirstOrDefaultAsync(c => c.CargoId == id);
            if(cargo == null)
            {
                Logger.LogWarning("Mo existe el cargo con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(cargo);
        }
    }
}