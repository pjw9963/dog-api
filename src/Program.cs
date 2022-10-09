using Amazon.S3;
using Npgsql;
using Microsoft.EntityFrameworkCore;

using dog_api.Models.App;
using dog_api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();

// bind database options using the options pattern
var dbOptions = new DataBaseOptions();
builder.Configuration.GetSection(DataBaseOptions.Section).Bind(dbOptions);

// build postgresql connection string and register dbcontext 
var dbBuilder = new NpgsqlConnectionStringBuilder()
{
    Password = dbOptions.Password,
    Database = dbOptions.DataBase,
    Port = dbOptions.Port,
    Username = dbOptions.Username,
    Host = dbOptions.Host
};

builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(dbBuilder.ConnectionString));

// add repos to di container
builder.Services.AddScoped<IDogImageFileRepo, S3DogImageFileRepo>();
builder.Services.AddScoped<IDogImageDataRepo, DogImagesEFCoreRepo>();
builder.Services.AddScoped<IDogImageRepo, DogImageRepo>();

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
