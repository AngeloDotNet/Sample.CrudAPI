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

        services.AddDbContextGenerics<DataDbContext>();
        services.AddDbContextUseSQLite<DataDbContext>(connectionString, string.Empty);

        services.AddTransient<IPeopleService, PeopleService>();

        services.AddFluentValidationService<Program>();
        services.AddSerilogSeqServices();

        services.AddHealthChecksSQLite<DataDbContext>("https://angelodotnet.github.io/", "Sample API", connectionString);
    }

    public void Configure(WebApplication app)
    {
        IWebHostEnvironment env = app.Environment;

        app.UseHttpsRedirection();
        app.AddSerilogConfigureServices();

        if (env.IsDevelopment())
        {
            app.AddUseSwaggerUI("Sample API");
        }

        app.UseRouting();
        app.UseHealthChecksConfigure();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}