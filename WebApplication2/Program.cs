using BussinessLayer.Interfaces.Repository;
using BussinessLayer.Interfaces.Services;
using BussinessLayer.Repository;
using BussinessLayer.Services;
using WebApplication2.Datalayer;
using WebApplication2.Datalayer.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


while (true)
{
    /*
    using (var db = new AppDbContext())
    {
        db.Database.EnsureCreated();
        var users = db.Users.ToList();
        foreach (var u in users)
        {
            db.Users.Remove(u);
        }
        db.SaveChanges();
        var user = new UserEntity {Id=1, Name = "Speller", Email = "sep@gogloe.org",PublicId=Guid.NewGuid() };
        db.Users.Add(user);
        db.SaveChanges();
        user = new UserEntity {Id=2, Name = "Spoofer", Email = "spo@google.online", PublicId = Guid.NewGuid() };
        db.Users.Add(user);
        db.SaveChanges();
        Console.WriteLine("\nUsers:");
        users = db.Users.ToList();
        foreach (var u in users)
        {
            Console.WriteLine($"ID:{u.Id},Jmeno:{u.Name},Emil:{u.Email}");
        }
        Console.WriteLine("");
    }*/
    break;
}
app.Run();