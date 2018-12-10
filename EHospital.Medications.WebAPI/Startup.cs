using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using EHospital.Medications.BusinessLogic.Contracts;
using EHospital.Medications.BusinessLogic.Services;
using EHospital.Medications.Data;
using EHospital.Medications.Model;
using Microsoft.AspNetCore.Mvc.Cors.Internal;

namespace EHospital.Medications.WebAPI
{
    /// <summary>
    /// Represent application startup settings and configuration.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// The connection string name defined in application settings.
        /// </summary>
        private const string CONNECTION_STRING_NAME = "EHospitalDB";

        ///* Swagger constants
        private const string VERSION = "v.1.0.0";
        private const string API_NAME = "EHospital.Medications";
        //*/

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the services.
        /// This method gets called by the runtime.
        /// This method is used to add services to the container.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = this.Configuration.GetConnectionString(CONNECTION_STRING_NAME);
            services.AddDbContext<MedicationDbContext>(options => options.UseSqlServer(connection));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepository<Drug>, Repository<Drug>>();
            services.AddScoped<IRepository<Prescription>, Repository<Prescription>>();
            services.AddScoped<IDrugService, DrugService>();
            services.AddScoped<IPrescriptionService, PrescriptionService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            ///* Swagger Setting
            Info info = new Info
            {
                Version = VERSION,
                Title = API_NAME,
                Description = "Micro-service provides REST request for managing drugs and prescriptions. "
                            + "Developed using ASP.Net Core WebAPI.",
                Contact = new Contact()
                {
                    Name = "Serhii Maksymchuk",
                    Email = "smakdealcase@gmail.com",
                    Url = "https://github.com/smoukiDev/EHospital.Medications"
                }
            };
            services.AddSwaggerGen(c => { c.SwaggerDoc(VERSION, info); });
            //*/

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()

                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("CorsPolicy"));
            });
        }

        /// <summary>
        /// This method gets called by the runtime.
        /// This method is used to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseMvc();

            ///* Swagger Setting
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"../swagger/{VERSION}/swagger.json", API_NAME);
            });
            //*/
        }
    }
}