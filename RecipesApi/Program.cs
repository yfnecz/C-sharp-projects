using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Recipes_api.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionstring = "User ID=sa;password=Educative@123;server=localhost;Database=recipes;TrustServerCertificate=True;";

builder.Services.AddDbContext<RecipeContext>(opt =>opt.UseSqlServer(connectionstring));


// --------------------------

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    c =>
    {
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,$"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    }
);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
