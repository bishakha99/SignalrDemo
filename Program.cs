using Microsoft.EntityFrameworkCore;
using Npgsql;
using SignalrDemo.DAL;
using SignalrDemo.HubConfig;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAllHeaders",
     builder =>
      {
        builder.AllowAnyHeader()
               .AllowAnyMethod()
               .WithOrigins("https://localhost:4200") // Replace with the Angular app's URL
               .AllowCredentials();
      });
});


builder.Services.AddDbContext<SignalrContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MyConnection"),
    //options => options.CommandTimeout(999)
    options => options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null)
), ServiceLifetime.Scoped);

builder.Services.AddScoped<IDbConnection>(sp =>
        new NpgsqlConnection(builder.Configuration.GetConnectionString("MyConnection")));

builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();
app.UseCors("AllowAllHeaders");
app.MapControllers();
app.UseEndpoints(endpoints =>
{
  endpoints.MapControllers();
  endpoints.MapHub<MyHub>("/toastr");
});


app.Run();
