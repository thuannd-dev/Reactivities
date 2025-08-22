using Application.Activities.Queries;
using Application.Core;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

//************************************************************************************************
//****************************** Add services to the container. ****************************
//************************************************************************************************

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();

/*
    Register various handlers from assembly - [kết quả biên dịch (compile) của project] containing given type - IRequestHandler
    That means you just need specify one handler, and all other handlers which similar derivered 
    will be discovered automatically (just in same project).
*/
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<GetActivityList.Handler>());

/*
    Register auto mapper and specify where the assembly - [kết quả biên dịch (compile) của project]
    is to register the mapping profiles with our application.
    So I don't have to specify each mapping profile in this project.
*/
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

var app = builder.Build();




//************************************************************************************************
//****************************** Configure the HTTP request pipeline. ****************************
//************************************************************************************************

/*
    Adds a CORS middleware to your web application pipeline to allow cross domain requests.
*/
app.UseCors(options => options.AllowAnyHeader()
                                .AllowAnyMethod()
                                .WithOrigins("http://localhost:3000", "https://localhost:3000"));
                                
/*
* MapControllers middleware provide the routing for application.
* It maps - pass the incoming HTTP requests to the appropriate- phù hợp controller actions.
* This is essential for the API to function correctly, allowing it to respond
* to requests with the defined routes in the controllers.
*/
app.MapControllers();

/*
    We can't get the service provider from the program class directly, (can't get it from class define it)
    so we use service locator pattern to get the service provider from the app.
*/
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync(); // Apply any pending migrations to the database.
    await DbInitializer.SeedData(context); // Seed the database with initial data.
}
catch (Exception ex)
{
    //ILogger param get the service that used ILogger interface.
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration or seeding the database.");
}

app.Run();
