using apbdcw11.DAL;
using apbdcw11.Services;
using Microsoft.EntityFrameworkCore;

namespace apbdcw11;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        string? connectionString = builder.Configuration.GetConnectionString("Default");

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddDbContext<DatabaseContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });

        builder.Services.AddScoped<IPrescriptionsService, PrescriptionsService>();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();

    }
}