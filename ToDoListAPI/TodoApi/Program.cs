using Microsoft.EntityFrameworkCore;
using TodoApi;

// ------------------------
// Minimal API Program.cs
// ------------------------
var builder = WebApplication.CreateBuilder(args);

// ✅ הוספת כל השירותים
builder.Services.AddDbContext<ToDoDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS – כבר יש לך! (אפשר להשאיר AllowAnyOrigin או לשנות ליותר ספציפי)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();   // עובד מצוין גם ככה
                                   // אם בא לך יותר בטוח: .WithOrigins("https://todolist-react-bgc4.onrender.com")
    });
});

var app = builder.Build();

// ✅ Middleware
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();   // ← חשוב! זה מפעיל את ה-CORS

// ✅ ROUTES – שיניתי רק את ה-path מ-/tasks ל-/api/todoitems
app.MapGet("/api/todoitems", async (ToDoDbContext db) =>
    await db.Items.ToListAsync());

app.MapPost("/api/todoitems", async (Item item, ToDoDbContext db) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/api/todoitems/{item.Id}", item);
});

app.MapPut("/api/todoitems/{id}", async (int id, Item updatedItem, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item == null) return Results.NotFound();

    item.Name = updatedItem.Name;
    item.IsComplete = updatedItem.IsComplete;
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

app.MapDelete("/api/todoitems/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item == null) return Results.NotFound();

    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();