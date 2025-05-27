using Application.Contratos;
using Application.CoberturaPlan;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Contexts;
using System.Text;
using Application.Data;
using Microsoft.Extensions.FileProviders;
using Domain.Utilitario;
using WebApiRetencionClientes.Configuration;
using sac_core.autorizacion;
using sac_core.excepcion;
using sac_core.logPeticion;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace WebApiRetencionClientes
{
    internal static class Program
    {
        private static void Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)  
            .MinimumLevel.Information()  
            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)  
            .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            var cultureInfo = new CultureInfo("es-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(cultureInfo);
                options.SupportedCultures = new[] { cultureInfo };
                options.SupportedUICultures = new[] { cultureInfo };
            });

            builder.Host.UseSerilog();

            builder.Services.Configure<BaseSetting>(builder.Configuration.GetSection("BaseSetting"));
            builder.Services.AddSingleton<IConfigurationService, ConfiguracionService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<Context>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("CadenaConexionCotizacionSAC"));
            });

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy => { policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
            }

            );
            builder.Services.AddMemoryCache();

            builder.Services.AddScoped<CoberturaPlanService>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CoberturaPlanQuery).Assembly));

            // Configuración de autenticación JWT
            //ConfigurationAuthSAC.ConfigurarJWT(builder.Services, builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            try
            {
                string imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
                if (Directory.Exists(imagesPath))
                {
                    app.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(imagesPath),
                        RequestPath = "/resources"
                    });
                }
                else
                {
                    Console.WriteLine("Warning: Images directory does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Warning Images Directory: " + ex.Message);
            }



            app.UseMiddleware<RegistroBitacoraSacMiddleware>();

            app.UseMiddleware<ExceptionMiddlewareSac>();

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseAuthentication();

            app.MapControllers();

            app.Services.GetRequiredService<IConfigurationService>().CargarConfiguraciones();

            using var scope = app.Services.CreateScope();

            app.Run();
        }
    }
}