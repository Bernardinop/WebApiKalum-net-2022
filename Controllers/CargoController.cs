using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;
using WebApiKalum.Dtos;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Cargo")]
    public class CargoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CargoController> Logger;
        private readonly IMapper Mapper;

        public CargoController(KalumDbContext _DbContext, ILogger<CargoController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CargoListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando el proceso de consulta de cargos");
            var cargos = await DbContext.Cargo.Include(c => c.CuentaPorCobrar).ToListAsync();
            if (cargos == null)
            {
                Logger.LogWarning("No existen cargos");
                return new NoContentResult();
            }
            List<CargoListDTO> tipoCargos = Mapper.Map<List<CargoListDTO>>(cargos);
            Logger.LogInformation("Se ejecutio la petición de forma existosa");
            return Ok(tipoCargos);
        }

        [HttpGet("{id}", Name = "GetCargo")]
        public async Task<ActionResult<CargoListDTO>> GetCargo(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + id);
            var cargo = await DbContext.Cargo.Include(c => c.CuentaPorCobrar).FirstOrDefaultAsync(c => c.CargoId == id);
            if (cargo == null)
            {
                Logger.LogWarning("No existe el cargo con el id " + id);
                return new NoContentResult();
            }
            CargoListDTO tipoCargo = Mapper.Map<CargoListDTO>(cargo);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(tipoCargo);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<CargoListDTO>>> GetPaginacion(int page)
        {
            var queryable = this.DbContext.Cargo.Include(c => c.CuentaPorCobrar).AsQueryable();
            var paginacion = new HttpResponsePaginacion<Cargo>(queryable, page);
            if (paginacion.Content == null && paginacion.Content.Count == 0)
            {
                Logger.LogWarning("No existen registros para paginar");
                return NoContent();
            }
            else
            {
                 Logger.LogInformation("Finalizando el proceso de busqueda por pagina de forma exitosa");
                return Ok(paginacion);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Cargo>> Post([FromBody] Cargo value)
        {
            Logger.LogDebug("Iniciando el proceso de agregar un nuevo cargo");
            value.CargoId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Cargo.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso para agregar un nuevo cargo");
            return new CreatedAtRouteResult("GetCargo", new { id = value.CargoId }, value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Cargo>> Delete(string id)
        {
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == id);
            if (cargo == null)
            {
                Logger.LogWarning($"No se encontro ningun cargo con el ID {id}");
                return NotFound();
            }
            else
            {
                DbContext.Cargo.Remove(cargo);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente el cargo con el ID {id}");
                return cargo;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Cargo value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion del cargo con el ID {id}");
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == id);
            if (cargo == null)
            {
                Logger.LogWarning($"No existe el cargo con el ID {id}");
                return BadRequest();
            }
            cargo.Descripcion = value.Descripcion;
            cargo.Prefijo = value.Prefijo;
            cargo.Monto = value.Monto;
            cargo.GeneraMora = value.GeneraMora;
            cargo.PorcentajeMora = value.PorcentajeMora;
            DbContext.Entry(cargo).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Los datos del cargo con el Id {id} han sido actualizados correctamente");
            return NoContent();
        }
    }
}