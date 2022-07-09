using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Inscripciones")]
    public class InscripcionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<InscripcionController> Logger;
        private readonly IMapper Mapper;

        public InscripcionController(KalumDbContext _DbContext, ILogger<InscripcionController> _Logger, IMapper _mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InscripcionListDTO>>> Get()
        {
            List<Inscripcion> inscripciones = null;
            Logger.LogDebug("Iniciando proceso de consulta de inscripciones");
            inscripciones = await DbContext.Inscripcion.ToListAsync();
            if (inscripciones == null || inscripciones.Count == 0)
            {
                Logger.LogWarning("No existen inscripciones");
                return new NoContentResult();
            }
            List<InscripcionListDTO> asignaciones = Mapper.Map<List<InscripcionListDTO>>(inscripciones);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(asignaciones);
        }

        [HttpGet("{id}", Name = "GetInscripcion")]
        public async Task<ActionResult<InscripcionListDTO>> GetInscripcion(string id)
        {
            Logger.LogDebug($"Iniciando el proceso de busqueda con el id {id}");
            var inscripcion = await DbContext.Inscripcion.FirstOrDefaultAsync(i => i.InscripcionId == id);
            if (inscripcion == null)
            {
                Logger.LogWarning($"No existe inscripción con el id {id}");
                return new NoContentResult();
            }
            InscripcionListDTO asignacion = Mapper.Map<InscripcionListDTO>(inscripcion);
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(asignacion);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<InscripcionListDTO>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando paginacion inscripciones");
            var queryable = DbContext.Inscripcion.AsQueryable();
            var paginacion = new HttpResponsePaginacion<Inscripcion>(queryable, page);
            if (paginacion.Content == null && paginacion.Content.Count == 0)
            {
                Logger.LogWarning("No existen registros para paginar");
                return NoContent();
            }
            else
            {
                List<InscripcionListDTO> inscripcion = Mapper.Map<List<InscripcionListDTO>>(paginacion.Content);
                Logger.LogInformation("Finalizando proceso de paginacion de inscripcion");
                return Ok(inscripcion);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Inscripcion>> Delete(string id)
        {
            Logger.LogDebug("Iniciando proceso de eliminación");
            Inscripcion inscripcion = await DbContext.Inscripcion.FirstOrDefaultAsync(i => i.InscripcionId == id);
            if (inscripcion == null)
            {
                Logger.LogWarning($"No se encontro ninguna inscripción con el id: {id}");
                return NotFound();
            }
            else
            {
                DbContext.Inscripcion.Remove(inscripcion);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente la inscripción con el id: {id}");
                return inscripcion;
            }
        }
        
        

        [HttpPost("Enrollments")]
        public async Task<ActionResult<ResponseEnrollmentDTO>> EnrollmentCreateAsync([FromBody] EnrollmentDTO value)
        {
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == value.NoExpediente);
            if (aspirante == null)
            {
                return NoContent();
            }
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if (carreraTecnica == null)
            {
                return NoContent();
            }
            bool respuesta = await CrearSolicitudAsync(value);
            if (respuesta == true)
            {
                ResponseEnrollmentDTO response = new ResponseEnrollmentDTO();
                response.HttpStatus = 201;
                response.Message = "El Proceso de inscripcion se ha realizado con exito";
                return Ok(response);
            }
            else
            {
                return StatusCode(503, value);
            }
        }
        private async Task<bool> CrearSolicitudAsync(EnrollmentDTO value)
        {
            bool proceso = false;
            ConnectionFactory factory = new ConnectionFactory();
            IConnection conexion = null;
            IModel channel = null;
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";
            try
            {
                conexion = factory.CreateConnection();
                channel = conexion.CreateModel();
                channel.BasicPublish("kalum.exchange.enrollment", "", null, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)));
                proceso = true;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
            finally
            {
                channel.Close();
                conexion.Close();
            }

            return proceso;
        }
    }
}