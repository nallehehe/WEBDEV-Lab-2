using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBDEV_Lab_2.Data;
using WEBDEV_Lab_2.Models;

namespace WEBDEV_Lab_2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<BookDbContext>(options => options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookReviewDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"));

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        /*if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }*/

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        //CRUD

        //Get book review by ID
        app.MapGet("/books/{id}", async ([FromRoute]int id, [FromServices]BookDbContext dbContext) =>
        {
            var book = await dbContext.Books.FindAsync(id);

            if (book == null)
            {
                return Results.NotFound("Unable to find that book review.");
            }

            return Results.Ok(book);
        });

        //Read all book reviews
        app.MapGet("/books", async ([FromServices]BookDbContext dbContext) =>
        {
            var books = await dbContext.Books.ToListAsync();
            return Results.Ok(books);
        });

        //Create book review
        app.MapPost("/book", async ([FromBody]Book book, [FromServices]BookDbContext dbContext) =>
        {
            dbContext.Books.Add(book);
            await dbContext.SaveChangesAsync();
            return Results.Ok(book);
        });

        //Update book review
        app.MapPut("/books/{id}", async ([FromRoute]int id, [FromBody]Book book, [FromServices]BookDbContext dbContext) =>
        {
            Book? updatedBook = await dbContext.Books.FindAsync(id);

            if (updatedBook == null)
            {
                return Results.NotFound("Unable to find that book review.");
            }

            updatedBook.Title = book.Title;
            updatedBook.Author = book.Author;
            updatedBook.Review = book.Review;

            await dbContext.SaveChangesAsync();
            return Results.Ok(book);
        });

        //Delete book review
        app.MapDelete("/books/{id}", async ([FromRoute]int id, [FromServices]BookDbContext dbContext) =>
        {
            var book = await dbContext.Books.FindAsync(id);

            if (book == null)
            {
                return Results.Ok();
            }

            dbContext.Books.Remove(book);
            await dbContext.SaveChangesAsync();
            return Results.Ok();
        });

        app.Run();
    }
}
