using DotNet_CSharp_React_EnergyApp_DS.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins,
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
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());


// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseCors(MyAllowSpecificOrigins);
//     app.UseSwaggerUI();
// }
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;


    var context = services.GetRequiredService<DeviceManagerContext>();
    if (context.Database.GetPendingMigrations().Any()) context.Database.Migrate();
}

app.Run();