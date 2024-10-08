using Microsoft.EntityFrameworkCore;
using HPlusSport.Web.Data;
using HPlusSport.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HPlusSport.Web.Data.HPlusSport;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("HPlusSportWebContextConnection") ?? throw new InvalidOperationException("Connection string 'HPlusSportWebContextConnection' not found.");

builder.Services.AddDbContext<WebContext>(options => options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<HPlusSportWebUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<WebContext>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("V�r� $ecret (not!)|V�r� $ecret (not!)|V�r� $ecret (not!)"));
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = key
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();