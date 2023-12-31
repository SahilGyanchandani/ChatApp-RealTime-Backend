using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Minimal_Chat_Application;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Minimal_Chat_Application.DataAccessLayer.Data;
using Minimal_Chat_Application.DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Minimal_Chat_Application.Hubs;
using Minimal_Chat_Application.BusinessLogicLayer.Interfaces;
using Minimal_Chat_Application.DataAccessLayer.Repository;
using Minimal_Chat_Application.BusinessLogicLayer.Services;
using StackExchange.Redis;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddScoped<IUserRegistrationRepository,UserRegistrationRepository>();


        builder.Services.AddScoped<IMessageRepository,MessageRepository>();
        builder.Services.AddScoped<IMessageService, MessageService>();

        builder.Services.AddScoped<RedisConnection>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "Doing Standard Authorization header using the Bearer Scheme (\"bearer{token}\")",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        // Add SignalR 
        builder.Services.AddSignalR();


        // Configure Redis connection
        builder.Services.AddSingleton(ConnectionMultiplexer.Connect("localhost:6379"));


        // For Entity Framework
        var connectionString = builder.Configuration.GetConnectionString("MySqlDb");
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        //for Identity
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
             .AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
         .AddJwtBearer(options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                     .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
                 ValidateIssuer = false,
                 ValidateAudience = false
             };
         })
          .AddGoogle(options =>
          {
              options.ClientId = (builder.Configuration.GetSection("Authentication:Google:ClientId").Value);
              options.ClientSecret = (builder.Configuration.GetSection("Authentication:Google:ClientSecret").Value);
          });

        builder.Services.AddCors(
            options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
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
        //app.UseSerilogRequestLogging();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseCors("AllowAll");
        app.UseRequestLoggingMiddleware();

        app.MapControllers();
        app.MapHub<ChatHub>("/chat");

        app.Run();
    }

}