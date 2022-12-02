using DotNet_CSharp_React_EnergyApp_DS.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            // policy.WithOrigins("http://localhost:3000").AllowAnyHeader()
            //     .AllowAnyMethod();
            policy.WithOrigins("http://localhost:4200").AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<DeviceManagerContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseCors(MyAllowSpecificOrigins);
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();