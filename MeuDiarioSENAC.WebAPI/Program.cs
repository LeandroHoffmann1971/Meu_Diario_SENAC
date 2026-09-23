using Microsoft.EntityFrameworkCore;
using MeuDiarioSENAC.Data;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Boa Noite!");

app.MapGet("/registros", () =>
{
    using var context = new MeuDiarioSENACContext();

    return context.Notas
        .AsNoTracking()
        .OrderBy(n => n.Id)
        .ToList();
});

app.Run();