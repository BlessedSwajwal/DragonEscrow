using Application;
using Infrastructure;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

{
    builder.Services.AddApplication()
        .AddInfrastructure(builder.Configuration);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "MyPolicy",
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://192.168.0.101:5173", "https://dealshield.vercel.app")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                          });
    });
}

{
    //Mapster
    var config = TypeAdapterConfig.GlobalSettings;
    config.Scan(typeof(Program).Assembly);
    config.Scan(typeof(DependencyInjectionRegisterApplication).Assembly);

    builder.Services.AddSingleton(config);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization();

app.Run();
