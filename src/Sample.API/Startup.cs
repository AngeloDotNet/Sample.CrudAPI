using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sample.API.Infrastructure;
using Sample.API.Infrastructure.Interfaces;
using Sample.API.Infrastructure.Repository;
using Sample.API.Service;

namespace Sample.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Sample API",
                Version = "v1"
            });
        });

        services.AddDbContextPool<DataDbContext>(optionsBuilder =>
        {
            var connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("Default");

            optionsBuilder.UseSqlite(connectionString);
        });

        services.AddScoped<DbContext, DataDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork<DbContext>>();
        services.AddScoped<IDatabaseRepository, DatabaseRepository<DbContext>>();
        services.AddScoped<ICommandRepository, CommandRepository<DbContext>>();

        services.AddTransient<IPeopleService, PeopleService>();
    }

    public void Configure(WebApplication app)
    {
        IWebHostEnvironment env = app.Environment;

        app.UseHttpsRedirection();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample API");
            });
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
