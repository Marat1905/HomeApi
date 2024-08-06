using FluentValidation.AspNetCore;
using HomeApi;
using HomeApi.Configuration;
using HomeApi.Contracts.Validators;
using HomeApi.Data.Context;
using HomeApi.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Runtime.ConstrainedExecution;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        /// <summary>
        /// Загрузка конфигурации из файла Json
        /// </summary>
       var Configuration= builder.Configuration.AddJsonFile("HomeOptions.json");

        // Подключаем автомаппинг
        var assembly = Assembly.GetAssembly(typeof(MappingProfile));
        builder.Services.AddAutoMapper(assembly);

        // регистрация сервиса репозитория для взаимодействия с базой данных
        builder.Services.AddSingleton<IDeviceRepository, DeviceRepository>();
        builder.Services.AddSingleton<IRoomRepository, RoomRepository>();

        string connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<HomeApiContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);

        // Подключаем валидацию
        builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddDeviceRequestValidator>());

        // Добавляем новый сервис
        builder.Services.Configure<HomeOptions>((IConfiguration)Configuration);

        // Загружаем только адресс (вложенный Json-объект))
        builder.Services.Configure<Address>(builder.Configuration.GetSection("Address"));

        // Нам не нужны представления, но в MVC бы здесь стояло AddControllersWithViews()
        builder.Services.AddControllers();
        // поддерживает автоматическую генерацию документации WebApi с использованием Swagger
        builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeApi", Version = "v1" }); });


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
    }
}

