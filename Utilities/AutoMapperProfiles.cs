using AutoMapper;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;

namespace WebApiKalum.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CarreraTecnicaCreateDTO, CarreraTecnica>();
            CreateMap<CarreraTecnica, CarreraTecnicaCreateDTO>();
            CreateMap<ExamenAdmision, ExamenAdmisionListDTO>();
            CreateMap<Aspirante, AspiranteListDTO>().ConstructUsing(e => new AspiranteListDTO { NombreCompleto = $"{e.Apellidos} {e.Nombres}" });
            CreateMap<Aspirante, AspiranteCarreraListDTO>().ConstructUsing(e => new AspiranteCarreraListDTO { NombreCompleto = $"{e.Apellidos} {e.Nombres}" });
            CreateMap<Inscripcion, InscripcionListDTO>();
            CreateMap<CarreraTecnica, CarreraTecnicaListDTO>();
            CreateMap<Inscripcion, AlumnoInscripcionDTO>();
            CreateMap<CuentaPorCobrar, CuentaPorCobrarListDTO>();
            CreateMap<Alumno, AlumnoListDTO>().ConstructUsing(e => new AlumnoListDTO { NombreCompleto = $"{e.Apellidos} {e.Nombres}" });
            CreateMap<Alumno, AlumnoCreateDTO>();
            CreateMap<AlumnoCreateDTO, Alumno>();
            CreateMap<ExamenAdmisionCreateDTO, ExamenAdmision>();
            CreateMap<ExamenAdmision, ExamenAdmisionCreateDTO>();
            CreateMap<Jornada, JornadaListDTO>();
            CreateMap<Jornada, JornadaCreateDTO>();
            CreateMap<JornadaCreateDTO, Jornada>();
            CreateMap<Cargo, CargoListDTO>();
            CreateMap<Cargo, CargoCreateDTO>();
            CreateMap<CargoCreateDTO, Cargo>();
            CreateMap<CuentaPorCobrar, CuentaPorCobrarCreateDTO>();
            CreateMap<CuentaPorCobrarCreateDTO, CuentaPorCobrar>();
            CreateMap<CarreraTecnica, InversionCarrera_CarreraTecnicaListDTO>();
            CreateMap<InversionCarreraTecnica, InversionCarreraListDTO>();
            CreateMap<InversionCarreraTecnica, InversionCarreraCreateDTO>();
            CreateMap<InversionCarreraCreateDTO, InversionCarreraTecnica>();
            CreateMap<ResultadoExamenAdmision, ResultadoExamenAdmisionListDTO>();
            CreateMap<InscripcionPago, InscripcionPagoListDTO>();
        }
    }
}