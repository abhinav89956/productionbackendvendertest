using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VenderTest.BarCode;
using VenderTest.CommonService;
using VenderTest.Data;
using VenderTest.Repository;
using VenderTest.Service;
using VenderTest.Service.VenderTest.Service;
using VenderTest.Services;

namespace VenderTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ========================
            // CONTROLLERS + SWAGGER
            // ========================
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ========================
            // SERVICES
            // ========================
            builder.Services.AddHttpClient<BarcodeGenerator>();

            builder.Services.AddScoped<Common>();
            builder.Services.AddScoped<BarcodeGenerator>();
            builder.Services.AddScoped<DapperDbContext>();

            // ========================
            // REPOSITORIES & SERVICES
            // ========================
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IForgetRepository, ForgetRepository>();
            builder.Services.AddScoped<IForgetService, ForgetService>();

            builder.Services.AddScoped<IItemRepository, ItemRepository>();
            builder.Services.AddScoped<IItemService, ItemService>();

            builder.Services.AddScoped<IVendorRepository, VendorRepository>();
            builder.Services.AddScoped<IVendorService, VendorService>();

            builder.Services.AddScoped<IBarCodeRepository, BarCodeRepository>();
            builder.Services.AddScoped<IBarCodeService, BarCodeService>();

            builder.Services.AddScoped<ISettingRepository, SettingRepository>();
            builder.Services.AddScoped<ISettingService, SettingService>();

            builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();

            builder.Services.AddScoped<IGenericRepository, GenericRepository>();

            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<IChatService, ChatService>();

            builder.Services.AddSingleton<IOnlineUserService, OnlineUserService>();

            // ========================
            // JWT AUTHENTICATION
            // ========================
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                    )
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/chathub"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthorization();

            // ========================
            // CORS (PRODUCTION SAFE)
            // ========================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularPolicy", policy =>
                {
                    policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            // ========================
            // SIGNALR
            // ========================
            builder.Services.AddSignalR();

            var app = builder.Build();

            // ========================
            // SWAGGER
            // ========================
            app.UseSwagger();
            app.UseSwaggerUI();

            // ========================
            // MIDDLEWARE ORDER
            // ========================
            app.UseCors("AngularPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            // ========================
            // ROUTES
            // ========================
            app.MapControllers();
            app.MapHub<ChatHub>("/chathub");

            app.MapGet("/", () => "Vender API is running 🚀");

            // ========================
            // RENDER PORT FIX
            // ========================
            var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
            app.Urls.Add($"http://0.0.0.0:{port}");

            app.Run();
        }
    }
}