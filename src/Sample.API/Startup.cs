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
        services.AddControllers(options => options.Filters.Add<ControllerApiExceptionFilter>())
            .AddSimpleJsonOptions();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGenConfig("Sample API", "v1", string.Empty);

        var connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("Default");

        services.AddDbContextServicesGenerics<DataDbContext>();
        services.AddDbContextForSQLite<DataDbContext>(connectionString, string.Empty);

        services.AddTransient<IPeopleService, PeopleService>();
        services.AddFluentValidationService<Program>();

        services.AddSerilogSeqServices();
        services.AddHealthChecksUISQLite<DataDbContext>(connectionString);
    }

    public void Configure(WebApplication app)
    {
        IWebHostEnvironment env = app.Environment;

        app.UseHttpsRedirection();
        app.AddSerilogConfigureServices();

        if (env.IsDevelopment())
        {
            app.UseSwaggerUI("Sample API");
        }

        app.UseRouting();
        app.UseHealthChecksUI();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}