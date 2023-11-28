using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCMovieInfo.Data;
//mwilliams:  Email Support
using Microsoft.AspNetCore.Identity.UI.Services;
using MVCMovieInfo.Services;
//End Email support 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

/* mwilliams:  Add Email support
   - Add EmailSender as a transient service.
   - Register the AuthMessageSenderOptions configuration instance.

     With a transient service, a new instance is provided every time an instance is requested 
     whether it is in the scope of same HTTP request or across different HTTP requests. 

     With a scoped service we get the same instance within the scope of a given HTTP request 
     but a new instance across different HTTP requests.
    
    Le mot de passe oublié est quelque chose de secondaire, il ne fait pas partie de la logique 
    principale de l'application, il est donc moins cher en termes de ressources
*/
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
//end email support

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
