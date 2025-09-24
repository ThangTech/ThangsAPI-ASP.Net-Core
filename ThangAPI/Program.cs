using Microsoft.EntityFrameworkCore;
using ThangAPI.Data;
using ThangAPI.Mapping;
using ThangAPI.Repositoty;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ThangDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("ThangConnectionString")));
builder.Services.AddScoped<IRegionRepository, SQLRegionRepositorycs>(); //Repository Pattern
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>(); //Repository Pattern
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>()); // Add automapper

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
