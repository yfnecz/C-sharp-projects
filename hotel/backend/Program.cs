using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using backend.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiContext>(opt => opt.UseSqlServer(builder.Configuration["Connection:DefaultConnection"]));

// configured jwtoptions to assign the secret key to `Secret` attribute
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

// added cors policy
builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin()
           .AllowAnyMethod()
            .AllowAnyHeader();


        });
    });


var app = builder.Build();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
