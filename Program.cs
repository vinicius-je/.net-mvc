using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Todo.Data;
using Todo.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<TodoModel>("todos");
    return builder.GetEdmModel();
}

var builder = WebApplication.CreateBuilder(args);

builder.Services
        .AddControllers()
        .AddOData(options => options
        .AddRouteComponents("odata", GetEdmModel())
        .Select()
        .Filter()
        .OrderBy()
        .SetMaxTop(20)
        .Count()
        .Expand());

builder.Services.AddDbContext<AppDbContext>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapControllers();
app.Run();
