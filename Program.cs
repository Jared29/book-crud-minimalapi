using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using BookManager.Models;
//using BookManager.DB;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Books") ?? "Data Source=Books.db";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<BookDb>(connectionString);
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Book API", 
        Description = "Keep track of books", 
        Version = "v1" 
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book API V1");
});

app.MapGet("/", () => "Hello World!");

// GET all books or specific book by id
app.MapGet("/books", async (BookDb db) => await db.Books.ToListAsync());
app.MapGet("/books/{id}", async (BookDb db, int id) => await db.Books.FindAsync(id));

// POST new book
app.MapPost("/book", async (BookDb db, Book book) => {
    await db.Books.AddAsync(book);
    await db.SaveChangesAsync();
    return Results.Created($"/book/{book.Id}", book);
});

// PUT book data by id
app.MapPut("/book/{id}", async(BookDb db, Book updatebook, int id) => {
    var book = await db.Books.FindAsync(id);
    if (book is null) return Results.NotFound();
    book.Title = updatebook.Title;
    book.Author = updatebook.Author;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// DELETE book by id
app.MapDelete("/book/{id}", async (BookDb db, int id) => {
    var book = await db.Books.FindAsync(id);
    if (book is null) return Results.NotFound();
    db.Books.Remove(book);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();
