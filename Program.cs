using System.Text;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Middleware;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DefaultConnection is missing.");

builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseSqlServer(connection));


// ============================================================
// IDENTITY
// ============================================================

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(o =>
    {
        o.Password.RequiredLength = 8;
        o.Password.RequireDigit = true;
        o.Password.RequireUppercase = true;
        o.Password.RequireLowercase = true;
        o.Password.RequireNonAlphanumeric = true;

        o.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// ============================================================
// JWT
// ============================================================

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is missing.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),

            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();


// ============================================================
// CONTROLLERS / SWAGGER
// ============================================================

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Campus Placement Management API",
        Version = "v1"
    });

    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Bearer token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    o.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference("Bearer", document),
                new List<string>()
            }
        });
});


// ============================================================
// CORS
// ============================================================

var origins =
    builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>()
    ?? Array.Empty<string>();

builder.Services.AddCors(o =>
    o.AddPolicy("Frontend", p =>
        p.WithOrigins(origins)
         .AllowAnyHeader()
         .AllowAnyMethod()
    )
);


// ============================================================
// REPOSITORIES
// ============================================================

builder.Services.AddScoped<ICollegeRepository, CollegeRepository>();
builder.Services.AddScoped<IPlacementOfficerRepository, PlacementOfficerRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();


// ============================================================
// SERVICES
// ============================================================

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICollegeService, CollegeService>();
builder.Services.AddScoped<IPlacementOfficerService, PlacementOfficerService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IEligibilityService, EligibilityService>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();


// ============================================================
// BUILD APPLICATION
// ============================================================

var app = builder.Build();


// ============================================================
// MIDDLEWARE
// ============================================================

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();

app.UseMiddleware<JwtRevocationMiddleware>();

app.UseAuthorization();

app.MapControllers();


// ============================================================
// DATABASE SEEDING
// ============================================================

await SeedAsync(app);

app.Run();


// ============================================================
// SEED METHOD
// ============================================================

static async Task SeedAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var serviceProvider = scope.ServiceProvider;


    // --------------------------------------------------------
    // DATABASE
    // --------------------------------------------------------

    var db = serviceProvider.GetRequiredService<ApplicationDbContext>();

    await db.Database.MigrateAsync();


    // --------------------------------------------------------
    // ROLE MANAGER
    // --------------------------------------------------------

    var roleManager =
        serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roleNames = new[]
    {
        "SuperAdmin",
        "PlacementOfficer",
        "CompanyHR",
        "Student"
    };


    foreach (var role in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var result = await roleManager.CreateAsync(
                new IdentityRole(role)
            );

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(
                        "; ",
                        result.Errors.Select(x => x.Description)
                    )
                );
            }
        }
    }


    // --------------------------------------------------------
    // USER MANAGER
    // --------------------------------------------------------

    var userManager =
        serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    var configuration =
        serviceProvider.GetRequiredService<IConfiguration>();


    // --------------------------------------------------------
    // SUPER ADMIN DETAILS
    // --------------------------------------------------------

    var email = configuration["Seed:SuperAdminEmail"]
        ?? throw new InvalidOperationException(
            "Seed:SuperAdminEmail is missing."
        );

    var password = configuration["Seed:SuperAdminPassword"]
        ?? throw new InvalidOperationException(
            "Seed:SuperAdminPassword is missing."
        );

    var fullName =
        configuration["Seed:SuperAdminName"]
        ?? "Platform Super Admin";


    // --------------------------------------------------------
    // CHECK WHETHER SUPER ADMIN ALREADY EXISTS
    // --------------------------------------------------------

    var admin =
        await userManager.FindByEmailAsync(email);


    // --------------------------------------------------------
    // CREATE SUPER ADMIN
    // --------------------------------------------------------

    if (admin == null)
    {
        admin = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            FullName = fullName,
            AccountStatus = AccountStatus.Approved
        };


        var result =
            await userManager.CreateAsync(
                admin,
                password
            );


        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(
                    "; ",
                    result.Errors.Select(x => x.Description)
                )
            );
        }


        await userManager.AddToRoleAsync(
            admin,
            "SuperAdmin"
        );
    }
}