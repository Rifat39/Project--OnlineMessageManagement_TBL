using OnlineMessageManagement.Data;
using OnlineMessageManagement.Services;

namespace OnlineMessageManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<ISocialServiceServices, SocialServiceServices>();
            builder.Services.AddTransient<ServiceUserRepository>();
            builder.Services.AddTransient<CustomerRepository>();
            builder.Services.AddTransient<SocialServiceRepository>();
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
                pattern: "{controller=SocialService}/{action=SocialServiceList}/{id?}");
                

            app.Run();
        }
    }
}