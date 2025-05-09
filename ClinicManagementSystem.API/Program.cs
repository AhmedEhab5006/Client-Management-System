using ClinicManagementSystem.BLL.Helpers;
using ClinicManagementSystem.BLL.Managers;
using ClinicManagementSystem.BLL.Managers.AuthManagers;
using ClinicManagementSystem.DAL.Database;
using ClinicManagementSystem.DAL.Models;
using ClinicManagementSystem.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IAuthManager, AuthManager>();
        builder.Services.AddScoped<IUserRepository , UserRepository>();
        builder.Services.AddScoped<IPasswordHandlerManager ,  PasswordHandlerManager>();
        builder.Services.AddScoped<IPatientRepository, PatientRepository>();
        builder.Services.AddScoped<IDoctorManager, DoctorManager>();
        builder.Services.AddScoped<IGetLoggedData, GetLoggedData>();
        builder.Services.AddScoped<IAppointmentRepository , AppointmentRepository>();
        builder.Services.AddScoped<IMedicalHistoryRepository, MedicalHistoryRepository>();
        builder.Services.AddHttpContextAccessor();
        

        builder.Services.AddDbContext<ProgramContext>(option =>

             option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



        builder.Services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = "";
            option.DefaultChallengeScheme = "";
        }).AddJwtBearer("", options =>
        {
            var securitykeystring = builder.Configuration.GetSection("SecretKey").Value;
            var securtykeyByte = Encoding.ASCII.GetBytes(securitykeystring);
            SecurityKey securityKey = new SymmetricSecurityKey(securtykeyByte);

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = securityKey,
                //ValidAudience = "url" ,
                //ValidIssuer = "url",
                ValidateIssuer = false,
                ValidateAudience = false,

                RoleClaimType = ClaimTypes.Role
            };
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() { Title = "Clinic Management System API", Version = "v1" });

            // 🔐 Add JWT Bearer token support
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer abcdefgh123456\""
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
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

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}