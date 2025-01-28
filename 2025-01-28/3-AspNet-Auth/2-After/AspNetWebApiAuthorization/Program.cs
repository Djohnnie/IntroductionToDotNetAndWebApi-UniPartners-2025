using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var issuer = "https://identity.azurewebsites.net/";
var audience = "https://identity.azurewebsites.net/";
var secret = "0ddf7075ef54453fa33eb446c9a88f2da3b6586abb304a5b8aa0666718886e79";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<MijnDbContext>(options =>
        {
            options.UseInMemoryDatabase("AspNetIdentity");
        });
builder.Services.AddIdentityCore<Gebruiker>(options =>
{
    options.Password = new PasswordOptions
    {
        RequiredLength = 4,
        RequireDigit = false,
        RequireLowercase = false,
        RequireUppercase = false,
        RequireNonAlphanumeric = false
    };
});

builder.Services.AddIdentity<Gebruiker, Rol>()
    .AddEntityFrameworkStores<MijnDbContext>()
    .AddDefaultTokenProviders()
    .AddPasswordValidator<PasswordValidator<Gebruiker>>();

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme =

    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = issuer,
        ValidAudience = audience,
        NameClaimType = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Name,
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () =>
{
    return Results.Ok();
}).WithName("GetStatus").WithOpenApi().AllowAnonymous();


app.MapGet("/secured", (HttpContext http) =>
{
    var message = $"Hallo, {http.User.Identity.Name}";
    return Results.Ok(message);
}).WithName("GetSecured").WithOpenApi().RequireAuthorization();


app.MapPost("/register", async (UserManager<Gebruiker> userManager, RegisterRequest request) =>
{
    var gebruiker = new Gebruiker
    {
        SecurityStamp = $"{Guid.NewGuid()}",
        UserName = request.UserName
    };

    var result = await userManager.CreateAsync(gebruiker, request.Password);

    if (!result.Succeeded)
    {
        throw new Exception("Registering user failed!");
    }

    return Results.Ok();
}).WithName("Register").WithOpenApi().AllowAnonymous();


app.MapPost("/login", async (UserManager<Gebruiker> userManager, LoginRequest request) =>
{
    var gebruiker = await userManager.FindByNameAsync(request.UserName);

    if (gebruiker == null)
    {
        return Results.NotFound();
    }

    var authenticated = await userManager.CheckPasswordAsync(gebruiker, request.Password);

    if (!authenticated)
    {
        return Results.Unauthorized();
    }

    var claims = new List<Claim>
        {
            new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sid, $"{gebruiker.Id}"),
            new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Name, $"{gebruiker.UserName}"),
            new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, $"{Guid.NewGuid()}"),
            new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, $"{gebruiker.Id}"),
            new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Iss, issuer),
            new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Aud, audience)
        };

    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

    var handler = new JwtSecurityTokenHandler();

    var token = handler.CreateToken(new SecurityTokenDescriptor
    {
        Issuer = issuer,
        Audience = audience,
        Expires = DateTime.Now.AddHours(4),
        Claims = claims.ToDictionary(k => k.Type, e => (object)e.Value),
        SigningCredentials = new SigningCredentials(
            authSigningKey, SecurityAlgorithms.HmacSha512)
    });

    var response = new LoginResponse
    {
        Token = handler.WriteToken(token)
    };

    return Results.Ok(response);
}).WithName("Login").WithOpenApi().AllowAnonymous();

app.Run();

public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class RegisterRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
}

public class Gebruiker : IdentityUser
{

}

public class Rol : IdentityRole
{

}

public class MijnDbContext : IdentityDbContext<Gebruiker>
{
    public MijnDbContext(DbContextOptions<MijnDbContext> options)
      : base(options)
    {
    }
}