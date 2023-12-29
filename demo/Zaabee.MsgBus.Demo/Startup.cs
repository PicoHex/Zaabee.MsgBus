namespace Zaabee.MsgBus.Demo;

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

        services.AddTransient<IZaabeeMsgBus>(
            _ => new ZaabeeMsgBus(services.BuildServiceProvider().GetService<IDbTransaction>())
        );
        services.AddSingleton<IZaabeeRabbitMqClient>(
            _ =>
                new ZaabeeRabbitMqClient(
                    new ZaabeeRabbitMqOptions
                    {
                        AutomaticRecoveryEnabled = true,
                        // Hosts = new List<string> { "192.168.78.130" },
                        Hosts =  ["127.0.0.1"],
                        UserName = "admin",
                        Password = "123",
                        Serializer = new NewtonsoftJson.Serializer()
                    }
                )
        );
        services.AddHostedService<ZaabeeMsgBusBackgroundService>();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Zaabee.MsgBus.Demo", Version = "v1" });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(
                c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zaabee.MsgBus.Demo v1")
            );
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
