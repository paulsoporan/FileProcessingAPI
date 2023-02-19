using FileProcessingAPI.BusinessLogic;
using FileProcessingAPI.Interfaces;
using FileProcessingAPI.PdfParser;
using FileProcessingAPI.Repositories;
using FileProcessingAPI.TextExtractor;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace FileProcessingAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "FileProcessingAPI Documentation",
                        Version = "v1",
                        Contact = new OpenApiContact
                        {
                            Name = "Paul Soporan",
                            Email = "sop.paul99@gmail.com"
                        }
                    });

                var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
                options.IncludeXmlComments(filePath);
            });
            builder.Services.AddScoped<IDbRepository, DbRepository>();
            builder.Services.AddScoped<IFileRepository, FileRepository>();
            builder.Services.AddScoped<IITextExtractor, ITextExtractor>();
            builder.Services.AddScoped<IPdfParserFactory, PdfParserFactory>();
            builder.Services.AddScoped<IProcessingBL, ProcessingBL>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                        options.SwaggerEndpoint("/swagger/v1/swagger.json",
                        "FileProcessingAPI")
                    );
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}