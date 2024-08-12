using Hangfire;
using HangFireUygulama.Context;
using HangFireUygulama.Models;
using HangFireUygulama.Service;
using Microsoft.EntityFrameworkCore;

//
var builder = WebApplication.CreateBuilder(args);

// Entity Framework Core yap�land�rmas�
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ILoggingService, LoggingService>();

// Hangfire yap�land�rmas�
builder.Services.AddHangfire(configuration =>
    configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer(); // Hangfire i�lerini i�lemek i�in gerekli hizmet
// Add services to the container.
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Hangfire Dashboard (iste�e ba�l� olarak)/7Planan i�leri izlelemizi sa�lar
app.UseHangfireDashboard("/hangfire");


// Basit bir i� tan�mlay�p zamanlamak /Bu i� planlad�ktan sonra ekrana merhaba yazd�r�r.
BackgroundJob.Schedule(() => Console.WriteLine("Merhaba "), TimeSpan.FromSeconds(10));
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Messages}/{action=Index}/{id?}");



void LogMessage(string message)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logMessage = new Message { MessageContent = message, CreatedAt = DateTime.Now };
        dbContext.Messages.Add(logMessage);
        dbContext.SaveChanges();
    }
}
app.Run();



/*

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

app.Run();*/
