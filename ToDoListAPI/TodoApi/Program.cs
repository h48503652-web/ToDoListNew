using Microsoft.EntityFrameworkCore;
using TodoApi;

// ------------------------
// Minimal API Program.cs
// ------------------------

var builder = WebApplication.CreateBuilder(args);

// ✅ הוספת כל השירותים לפני Build()
builder.Services.AddDbContext<ToDoDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

// ✅ עכשיו אפשר לבנות את האפליקציה
var app = builder.Build();

// ✅ Middleware
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

// ✅ ROUTES
app.MapGet("/tasks", async (ToDoDbContext db) =>
    await db.Items.ToListAsync()
);

app.MapPost("/tasks", async (Item item, ToDoDbContext db) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{item.Id}", item);
});

app.MapPut("/tasks/{id}", async (int id, Item updatedItem, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item == null) return Results.NotFound();

    item.Name = updatedItem.Name;
    item.IsComplete = updatedItem.IsComplete;
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

app.MapDelete("/tasks/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item == null) return Results.NotFound();

    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
