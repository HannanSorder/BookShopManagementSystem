using Microsoft.EntityFrameworkCore;
using BookShop.Server.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ❌ এই line টি REMOVE করুন:
// builder.WebHost.UseWebRoot("wwwroot");

builder.Services.AddDbContext<BookShopAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookShopAPIContext") ?? throw new InvalidOperationException("Connection string 'BookShopAPIContext' not found.")));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();