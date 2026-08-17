using Microsoft.EntityFrameworkCore;
using rybank.estudo.Data;
using rybank.estudo.Interfaces;
using rybank.estudo.Services;

var builder = WebApplication.CreateBuilder(args);


var connectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>( options =>
{
    options.UseNpgsql(connectionStrings);
});

builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBankService, BankService>();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
