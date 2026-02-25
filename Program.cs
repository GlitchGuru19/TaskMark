using Scalar.AspNetCore;
using Task.API.Data; 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Adding connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if(string.IsNullOrEmpty(connectionString))
    throw new Exception("Connection is null or invalid");

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlite(connectionString);
});

// Authentication and Authorization stuff...
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>().AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // initiates the scalar docs.
    app.MapScalarApiReference();
}

// Map the middleware
app.MapIdentityApi<ApplicationUser>();

app.UseHttpsRedirection();

app.UseAuthorization(); // always comes before the controller

app.MapControllers();

app.Run();