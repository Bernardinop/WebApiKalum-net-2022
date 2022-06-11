using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class ExamenAdmisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ExamenAdmisionController> Logger;

        public ExamenAdmisionController(KalumDbContext dbContext, ILogger<ExamenAdmisionController> _Logger)
        {
            this.DbContext = dbContext;
            this.Logger = _Logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamenAdmision>>> Get()
        {
            List<ExamenAdmision> examenes = null;
            Logger.LogDebug("Iniciando proceso de consulta examenes admisión");
            examenes = await DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).ToListAsync();
            if (examenes == null || examenes.Count == 0)
            {
                Logger.LogWarning("No existen examenes de admisión");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(examenes);
        }

        [HttpGet("{id}", Name = "GetExamenAdmision")]
        public async Task<ActionResult<ExamenAdmision>> GetExamenAdmision(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + id);
            var examen = await DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).FirstOrDefaultAsync(ea => ea.ExamenId == id);
            if (examen == null)
            {
                Logger.LogWarning("No existe el examen de admisión con el id " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(examen);
        }
    }
}