using Microsoft.EntityFrameworkCore;
using sample_web_app;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");



builder.Services.AddDbContext<AppDbContext>(Options => Options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

//API

app.MapGet("/", () => "Welcome to the Student API!");

app.MapGet("/students", async (AppDbContext dbContext) =>
{
    return await dbContext.Students.ToListAsync();
});

app.MapPost("/students", async (AppDbContext dbContext, Student student) =>
{
    dbContext.Students.Add(student);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/students/{student.Id}", student);
});

app.MapGet("/students/{id}", async (AppDbContext dbContext, int id) =>
{
    var student = await dbContext.Students.FindAsync(id);
    return student is not null ? Results.Ok(student) : Results.NotFound();
});

app.Run();
