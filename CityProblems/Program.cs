using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json.Serialization;
using CityProblems.DataAccess;
using Microsoft.EntityFrameworkCore;
using CityProblems.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//authorization services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/Account/SignIn");
builder.Services.AddAuthorization();

//not cycling json converting
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//db connection
var connectionString = builder.Configuration.GetConnectionString("CityProblemsConnection");
builder.Services.AddDbContext<CityProblemsDbContext>(opt => opt.UseLazyLoadingProxies().UseNpgsql(connectionString,
    b => b.MigrationsAssembly("CityProblems")));

//add services
builder.Services.AddTransient<IUserService,UserService>();
builder.Services.AddTransient<IIssueService,IssueService>();
builder.Services.AddTransient<IMessageService, MessageService>();
builder.Services.AddTransient<ICategoryService,CategoryService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
