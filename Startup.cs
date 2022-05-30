using Microsoft.AspNetCore.Builder;
using project_dc_system.Models;
using Microsoft.EntityFrameworkCore;

namespace project_dc_system
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Conexão com SQL Server
            //services.AddDbContext<VendasContext>();

            // Conexão com PostgreSQL
            services.AddDbContext<VendasContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("VendasContext")));
        }
    }
}
