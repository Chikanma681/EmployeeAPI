var builder = WebApplication.CreateBuilder(args);

var employees = new List<Employee>
{
    new Employee { Id = 1, FirstName = "John", LastName = "Doe" },
    new Employee { Id = 2, FirstName = "Jane", LastName = "Smith" }
};
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


var employeeRoute = app.MapGroup("/employees");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

employeeRoute.MapGet("/", () => {
    return Results.Ok(employees);
});

employeeRoute.MapGet("/{id:int}", (int id) => {
    var employee = employees.SingleOrDefault(e=> e.Id == id);

    if(employee == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(employee);
});

employeeRoute.MapPost("/", (Employee employee) => {
    employee.Id = employees.Max(e => e.Id) + 1;
    employees.Add(employee);
    return Results.Created($"/{employee.Id}", employee);
});

app.Run();