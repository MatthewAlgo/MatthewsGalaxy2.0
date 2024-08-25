using MatthewsGalaxy.Server.Models;
using MatthewsGalaxy.WorkerMicroservice.Interfaces;
using MatthewsGalaxy.WorkerMicroservice.Repository;
using MatthewsGalaxy.WorkerMicroservice.Services;
using MatthewsGalaxy.WorkerMicroservice.Workers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthewsGalaxy.WorkerMicroservice
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("MatthewsGalaxyConnectionString")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<EmailService>();
            services.AddScoped<IApiService, ApiService>();
            services.AddScoped<DataService>();

            // Register HttpClient
            services.AddHttpClient<ApiService>();

            services.AddScoped<IGenericRepository<BlogPost>, GenericRepository<BlogPost>>();
            services.AddScoped<IGenericRepository<Subscriber>, GenericRepository<Subscriber>>();

            services.AddHostedService<NewPostsEmailSender>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MatthewsGalaxy Microservice API", Version = "v1" });
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MatthewsGalaxy Microservice API V1");
            });
        }
    }
}
