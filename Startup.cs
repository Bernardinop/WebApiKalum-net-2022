namespace WebApiKalum
{
    public class Startup
    {
        public IConfiguration Configuration { get;}
        public Startup(IConfiguration _Configuration)
        {
            this.Configuration = _Configuration;
        }
        public void ConfigureServices(IServiceCollection _services)
        {
            _services.AddControllers();
            //_services.AddContext<>
        }

    }
}