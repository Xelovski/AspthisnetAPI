using BussinessLayer.Interfaces.Repository;
using BussinessLayer.Interfaces.Services;
using BussinessLayer.Repository;
using BussinessLayer.Services;
using Microsoft.OpenApi.Models;
using WebApplication2.Datalayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });

    c.EnableAnnotations(); //  IMPORTANT
});

builder.Services.AddCors(option =>
{
    option.AddPolicy("ReactLocalhost", policy => //ReectLocalPain
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
}
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors("ReactLocalhost");//ReectLocalPain

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
