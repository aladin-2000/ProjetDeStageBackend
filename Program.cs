using backEnd.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 33));

builder.Services.AddControllers();

builder.Services.AddDbContext<AssetDbContexte>(dbContextOptions => dbContextOptions
    .UseMySql(connectionString, serverVersion)
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name:MyAllowSpecificOrigins, 
        builder =>
    {
        builder.WithOrigins("http://localhost",
            "http://localhost:4200",
            "http://192.168.1.7:8080",
            "https://localhost:7230",
            "http://localhost:90")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();
