using EnterpriseProject.Services;
using EnterpriseProject.Services.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection")));

builder.Services.AddScoped<IResumeServices, ResumeRepository>();
builder.Services.AddScoped<IProjectServices, ProjectRepository>();
builder.Services.AddScoped<IUserServices, UserRepository>();
builder.Services.AddScoped<IProfileServices, ProfileRepository>();
builder.Services.AddScoped<ICommentServices, CommentRepository>();


//Adding authentication here
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Dashboard/Login";
        options.LogoutPath = "/Dashboard/Logout";
        options.Cookie.Name = "AuthCookie";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
