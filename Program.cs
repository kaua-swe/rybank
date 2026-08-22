using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Interfaces.Account;
using src.Interfaces.Auth;
using src.Interfaces.Bank;
using src.Interfaces.Pix;
using src.Interfaces.Register;
using src.Interfaces.Ticket;
using src.Services.Account;
using src.Services.Auth;
using src.Services.Bank;
using src.Services.Pix;
using src.Services.Register;
using src.Services.Ticket;

var builder = WebApplication.CreateBuilder(args);

var connectionStrigs = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>( options =>
{
    options.UseNpgsql(connectionStrigs);
});

builder.Services.AddControllers();

builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<IPixService, PixService>();
builder.Services.AddScoped<ITicketService, TicketService>();


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
