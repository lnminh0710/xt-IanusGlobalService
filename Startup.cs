using IanusGlobalServiceApi.Models;
using IanusGlobalServiceApi.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace IanusGlobalServiceApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
                    //.AddNewtonsoftJson();

            #region Injection
            //https://stackoverflow.com/questions/38138100/addtransient-addscoped-and-addsingleton-services-differences
            /*
             - Transient: objects are always different; a new instance is provided to every controller and every service.
             - Scoped: objects are the same within a request, but different across different requests.
             - Singleton: objects are the same for every object and every request.
             */
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region appsettings.json
            // Register the IConfiguration instance which AppSettings binds against.
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.TryAddSingleton<IAppServerSetting, AppServerSetting>();
            #endregion

            services.AddTransient<IDalAdoService, DalAdoService>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
