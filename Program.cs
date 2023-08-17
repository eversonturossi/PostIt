var builder = WebApplication.CreateBuilder(args);

Log.Logger = new Serilog.LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog((builderContext, configureLogger) => configureLogger.ReadFrom.Configuration(builder.Configuration));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDependencyInjectionMiddleware();
builder.Services.AddQuartzMiddleware();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
