using Microsoft.EntityFrameworkCore;
using MinimalAPI.WebAPI.Models.DataContexts;
using MinimalAPI.WebAPI.Models.Entities;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;

services.AddEndpointsApiExplorer();

services.AddDbContext<TodoDbContext>(cfg =>
{
    cfg.UseInMemoryDatabase("MinimalAPITests");
});

services.AddSwaggerGen();

IWebHostEnvironment env = builder.Environment;
WebApplication app = builder.Build();

app.MapGet("/", () => "Hello World!");
// GetAll
app.MapGet("/gettodos", async (TodoDbContext db) => await db.Todos.ToListAsync());
// GetById
app.MapGet("/gettodo/{id}", async (TodoDbContext db, int id) =>
await db.Todos.FirstOrDefaultAsync(t => t.Id == id) is Todo todo ? Results.Ok(todo) : Results.NotFound()
);
// Post
app.MapPost("/posttodo", async (TodoDbContext db, Todo todo) =>
{

    await db.Todos.AddAsync(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/gettodo/{todo.Id}", todo);
});
// Put
app.MapPut("/posttodo", async (TodoDbContext db, Todo todo, int id) =>
{
    Todo? entity = await db.Todos.FindAsync(id);

    if (entity is null)
    {
        Results.NotFound();
    }

    db.Todos.Update(todo);
    await db.SaveChangesAsync();

    //return Results.NoContent();
    return Results.Accepted($"/gettodo/{todo.Id}", todo);
});
// Delete
app.MapDelete("/deletetodo/{id}", async (int id, TodoDbContext db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }

    return Results.BadRequest();
});

if (env.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
