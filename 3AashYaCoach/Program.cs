    using _3AashYaCoach._3ash_ya_coach.Core.DI;
    using _3AashYaCoach._3ash_ya_coach.Services;
    using _3AashYaCoach._3ash_ya_coach.Services.NotificationService;
    using _3AashYaCoach.Models.Context;
    using FirebaseAdmin;
    using Google.Apis.Auth.OAuth2;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using System.Text;
    using Microsoft.AspNetCore.Http;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddDbContext<AppDbContext>(options =>
        //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    options.UseSqlServer(builder.Configuration.GetConnectionString("ServerConnection")));


builder.Services.AddControllersWithViews();  

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "3AashYaCoach", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Enter JWT Bearer token only (no 'Bearer ' prefix)",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
  .AddCookie(options =>
    {
        options.LoginPath = "/Identity/Signin";
        options.AccessDeniedPath = "/Identity/AccessDenied";
        options.LogoutPath = "/Identity/Logout";
        options.Cookie.Name = "3AashYaCoach.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // تحقق إن الاتصال رايح للـ Hub
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });
    var firebasePath = Path.Combine(Directory.GetCurrentDirectory(),
        "3ash_ya_coach/Services/NotificationService/Firebase/fitness-ce0ca-firebase-adminsdk-fbsvc-76052137ff.json");

    FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(firebasePath)
        });
    builder.Services.AddSession();
    builder.Services.RegisterCustomServices();
    builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // إذا كنت تستخدم توكن مع الكوكيز
    });
});


builder.Services.AddSignalR()
        .AddHubOptions<ChatHub>(options =>
        {
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
        });

    var app = builder.Build();
app.UseCors("AllowFrontend");
app.UseSession();

    app.UseStaticFiles();
    //if (app.Environment.IsProduction())
    if (app.Environment.IsDevelopment())
    {
    app.UseSwagger();
    app.UseSwaggerUI();
    }
    app.UseRouting();
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapHub<NotificationHub>("/notificationHub");

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHub<ChatHub>("/chathub");
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    });

    app.Run();
