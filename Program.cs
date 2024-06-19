using Microsoft.EntityFrameworkCore;
using minimal_api_crud.Data;
using minimal_api_crud.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("minimal-api-crud"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/v1/customers/", async (AppDbContext dbContext) =>
    await dbContext.Customers.AsNoTracking().ToListAsync());

app.MapGet("/v1/customers/{id}", async (int id, AppDbContext dbContext) =>
    await dbContext.Customers.AsNoTracking().FindAsync(id) is Customer customer ? Results.Ok(customer) : Results.NotFound());

app.MapPost("/v1/customers/", async (Customer model, AppDbContext dbContext) =>
{
    await dbContext.Customers.AddAsync(model);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/v1/customers/{model.Id}", model);    
});

app.MapPut("/v1/customers/{id}", async (int id, Customer model, AppDbContext dbContext) =>
{
    var customer = await dbContext.Customers.FindAsync(id);

    if (customer is null) return Results.NotFound();

    customer.Name = model.Name;
    customer.Email = model.Email;

    await dbContext.SaveChangesAsync();

    return Results.Ok(customer);    
});

app.MapDelete("/v1/customers/{id}", async (int id, AppDbContext dbContext) =>
{
    if (await dbContext.Customers.FindAsync(id) is Customer customer)
    {
        dbContext.Customers.Remove(customer);
        await dbContext.SaveChangesAsync();
        
        return Results.Ok(customer);
    }

    return Results.NotFound();    
});

app.Run();
