using Microsoft.EntityFrameworkCore;
using rybank.estudo.Data;
using rybank.estudo.Interfaces;
using rybank.estudo.Services;
using rybank.Interfaces;
using rybank.Interfaces.Account;
using rybank.Services;
using rybank.Services.Account;

var builder = WebApplication.CreateBuilder(args);


var connectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>( options =>
{
    options.UseNpgsql(connectionStrings);
});

builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<IPixService, PixService>();
builder.Services.AddScoped<IAccountService, AccountService>();

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
