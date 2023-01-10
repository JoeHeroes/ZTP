using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ZTP.Models;
using ZTP.Seeder;

var builder = WebApplication.CreateBuilder();

builder.Services.AddControllersWithViews();

builder.Services.AddSession();


var authentications = new Authentication();

builder.Configuration.GetSection("Authentication").Bind(authentications);

//Autoorization
builder.Services.AddSingleton(authentications);


builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authentications.JwtIssuer,
        ValidAudience = authentications.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authentications.JwtKey)),
    };
});


//Sedder
builder.Services.AddScoped<ZTPSeeder>();

//Hasser
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

//ContextAccessor
builder.Services.AddHttpContextAccessor();

//DbContext
builder.Services.AddDbContext<ZTPDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseSession();

var scope = app.Services.CreateScope();

var seeder = scope.ServiceProvider.GetRequiredService<ZTPSeeder>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

seeder.Seed();

app.UseRouting();


app.UseAuthorization();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
