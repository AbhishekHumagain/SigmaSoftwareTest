using SigmaSoftware.Application;
using SigmaSoftware.Infrastructure.Configurations;
using SigmaSoftware.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);


builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment);

builder.Services.AddHttpContextAccessor();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
const string allowedOrigins = "AllowedOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(allowedOrigins,
        policy =>
        {
            policy.WithOrigins(builder.Configuration.GetSection("App:CorsOrigins").Get<string[]>())
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseMigrationsEndPoint();
app.UseCors(allowedOrigins);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<SigmaDbContextInitializer>();
    await initializer.InitialiseAsync();
    await initializer.SeedAsync();
}

app.UseHealthChecks("/health");

app.MapControllers();


app.Run();