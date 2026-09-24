using MeuDiarioSENAC.Business;
using MeuDiarioSENAC.Data;
using MeuDiarioSENAC.Data.Repositories;
using MeuDiarioSENAC.Model;
using MeuDiarioSENAC.Service;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

using (var context = new MeuDiarioSENACContext())
{
    context.GarantirBancoETabela();
}

var registroService = new RegistroService(new NotaRepository(), new RegistroBusiness());
var registrosGroup = app.MapGroup("/registros");

app.MapGet("/", () => "Boa Noite!");

registrosGroup.MapGet("/", (int? id, string? titulo, DateTime? data, string? conteudo) =>
{
    try
    {
        return Results.Ok(registroService.Listar(id, titulo, data, conteudo));
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

registrosGroup.MapPost("/", (CreateNotaRequest request) =>
{
    try
    {
        var nota = registroService.Criar(request.Titulo, request.Conteudo);
        var response = new NotaResponse(nota.Id, nota.Titulo, nota.Data, nota.Conteudo);
        return Results.Created($"/registros/{nota.Id}", response);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

registrosGroup.MapPut("/{id:int}", (int id, UpdateNotaRequest request) =>
{
    try
    {
        if (registroService.BuscarPorId(id) is null)
        {
            return Results.NotFound();
        }

        registroService.Atualizar(id, request.Titulo, request.Conteudo);
        var nota = registroService.BuscarPorId(id)!;
        return Results.Ok(new NotaResponse(nota.Id, nota.Titulo, nota.Data, nota.Conteudo));
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

registrosGroup.MapDelete("/{id:int}", (int id) =>
{
    return registroService.Excluir(id)
        ? Results.NoContent()
        : Results.NotFound();
});



app.Run();

/// <summary>Payload used to create a diary record.</summary>
public sealed record CreateNotaRequest(string Titulo, string Conteudo);

/// <summary>Payload used to update a diary record.</summary>
public sealed record UpdateNotaRequest(string Titulo, string Conteudo);

/// <summary>Diary record returned by the API.</summary>
public sealed record NotaResponse(int Id, string Titulo, DateTime Data, string Conteudo);