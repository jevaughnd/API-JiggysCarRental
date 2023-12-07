using API_JiggysCarRental;
using API_JiggysCarRental.DATA;
using API_JiggysCarRental.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//-------------------------------------------------------------------------------------------------------


// Dipendency Injection for ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("JevConnection")));



//----------------------------------------  IDENTITY SERVICES  ----------------------------------------//


//Add Identity User // Store -------
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 5;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();



//add Auth Service  ------
builder.Services.AddTransient<IAuthService, AuthService>();



//Authorization -------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.Requirements.Add(new AdminAcces())); //add new
});



//Authentication ------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JWTConfig: Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JWTConfig: Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWTConfig:Key").Value))
    };
});



//------------------------------------------------------------------------------------------------------///






var app = builder.Build();



//Enable the serving of static files: 1
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath,
                                            "api/server/VehicleImgFileUploads")),
    RequestPath = "/images/api/server/VehicleImgFileUploads" // Change this to the URL path you want to use
});



//Enable the serving of static files: 2
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath,
                                            "api/server/Updated-Vehicle-Imgs")),
    RequestPath = "/images/api/server/Updated-Vehicle-Imgs" // Change this to the URL path you want to use
});








// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Added
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace API_JiggysCarRental
{
    class AdminAcces : IAuthorizationRequirement
    {
    }
}