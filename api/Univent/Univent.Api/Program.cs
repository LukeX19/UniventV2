using Microsoft.EntityFrameworkCore;
using Univent.Api.Extensions;
using Univent.Api.Middlewares;
using Univent.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.RegisterAuthentication();
builder.RegisterAWSS3Storage();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddMediatR();
builder.Services.AddAutoMapper();
builder.Services.AddRepositories();
builder.Services.AddBusinessServices();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLServer"))
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AngularClient");

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
