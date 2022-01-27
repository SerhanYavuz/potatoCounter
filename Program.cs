using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using PotatoCounter.Interfaces;
using PotatoCounter.Messaging;
using PotatoCounter.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication("Bearer")
.AddIdentityServerAuthentication("Bearer", options => {
    options.ApiName = "weatherapi";
    options.Authority= "https://localhost:7249";
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqConfiguration"));
builder.Services.AddSingleton<IRabbitMqConfiguration>(sp => sp.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value);   

builder.Services.AddSingleton<IPotatoPostSender,PotatoPostSender>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();//eğer bu sırayla yazılmazsa loop oluyor
app.UseAuthorization();

app.MapControllers();


app.Run();
