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
        /// �������� ������������ �� ����� Json
        /// </summary>
       var Configuration= builder.Configuration.AddJsonFile("HomeOptions.json");

        // ���������� �����������
        var assembly = Assembly.GetAssembly(typeof(MappingProfile));
        builder.Services.AddAutoMapper(assembly);

        // ����������� ������� ����������� ��� �������������� � ����� ������
        builder.Services.AddSingleton<IDeviceRepository, DeviceRepository>();
        builder.Services.AddSingleton<IRoomRepository, RoomRepository>();

        string connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<HomeApiContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);

        // ���������� ���������
        builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddDeviceRequestValidator>());

        // ��������� ����� ������
        builder.Services.Configure<HomeOptions>((IConfiguration)Configuration);

        // ��������� ������ ������ (��������� Json-������))
        builder.Services.Configure<Address>(builder.Configuration.GetSection("Address"));

        // ��� �� ����� �������������, �� � MVC �� ����� ������ AddControllersWithViews()
        builder.Services.AddControllers();
        // ������������ �������������� ��������� ������������ WebApi � �������������� Swagger
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

