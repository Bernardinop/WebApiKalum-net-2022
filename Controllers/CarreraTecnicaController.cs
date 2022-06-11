using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/CarrerasTecnicas")]
    public class CarreraTecnicaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CarreraTecnicaController> Logger;
        public CarreraTecnicaController(KalumDbContext _Dbcontext, ILogger<CarreraTecnicaController> _Logger)
        {
            this.DbContext = _Dbcontext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarreraTecnica>>> Get()
        {
            List<CarreraTecnica> carrerasTecnicas = null;
            //Tarea 1
            Logger.LogDebug("Iniciando proceso de consulta de carreras tecnicas en la base de datos");
            carrerasTecnicas = await DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(c => c.Inscripciones).ToListAsync();
            //Tarea 2
            if (carrerasTecnicas == null || carrerasTecnicas.Count == 0)
            {
                Logger.LogWarning("No existe carreras tecnicas");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(carrerasTecnicas);
        }

        [HttpGet("{id}", Name = "GetCarreraTecnica")]
        public async Task<ActionResult<CarreraTecnica>> getCarreraTecnica(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + id);
            var carrera = await DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(c => c.Inscripciones).FirstOrDefaultAsync(ct => ct.CarreraId == id);
            if(carrera == null)
            {
                Logger.LogWarning("No existe la carrera tecnica con el id" + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok (carrera);
        }
    }
}