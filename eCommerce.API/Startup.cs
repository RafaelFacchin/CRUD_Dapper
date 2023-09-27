using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method get`s called by the runtime. Use this method to add services to the container

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "eCommerce.API", Version = "v1" });
            });
        }

        //This method gets called by the runtime. Use this method to configure the HTTP request

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) //VERIFICACAO DE ERROS
            {
                //Middleway
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "eCommerce.API v1"));
            }

            app.UseHttpsRedirection();//Redireciona de HTTP para HTTPS

            app.UseRouting();//USO de rotas

            app.UseAuthorization();//USO de autorizacao

            app.UseEndpoints(endpoints => //USO DE ENDPOINTS (ENDERECOS DE Acesso URL)
            {
                endpoints.MapControllers();
            });
        }
    }
}
