using Microsoft.EntityFrameworkCore;
using MeuDiarioSENAC.Model;

namespace MeuDiarioSENAC.Data.Repositories;

public class NotaRepository
{
    public List<Nota> Listar(int? id = null, string? titulo = null, DateTime? data = null, string? conteudo = null)
    {
        using var context = new MeuDiarioSENACContext();
        var query = context.Notas.AsNoTracking().AsQueryable();

        if (id.HasValue)
        {
            query = query.Where(n => n.Id == id.Value);
        }

        if (!string.IsNullOrWhiteSpace(titulo))
        {
            query = query.Where(n => n.Titulo.Contains(titulo));
        }

        if (data.HasValue)
        {
            var inicio = data.Value.Date;
            var fim = inicio.AddDays(1);
            query = query.Where(n => n.Data >= inicio && n.Data < fim);
        }

        if (!string.IsNullOrWhiteSpace(conteudo))
        {
            query = query.Where(n => n.Conteudo.Contains(conteudo));
        }

        return query
            .OrderBy(n => n.Id)
            .ToList();
    }

    public Nota? BuscarPorId(int id)
    {
        using var context = new MeuDiarioSENACContext();
        return context.Notas.Find(id);
    }

    public Nota Adicionar(Nota nota)
    {
        using var context = new MeuDiarioSENACContext();
        context.Notas.Add(nota);
        context.SaveChanges();
        return nota;
    }

    public bool Atualizar(Nota nota)
    {
        using var context = new MeuDiarioSENACContext();
        var existente = context.Notas.Find(nota.Id);

        if (existente is null)
        {
            return false;
        }

        existente.Titulo = nota.Titulo;
        existente.Conteudo = nota.Conteudo;
        context.SaveChanges();
        return true;
    }

    public bool Excluir(int id)
    {
        using var context = new MeuDiarioSENACContext();
        var existente = context.Notas.Find(id);

        if (existente is null)
        {
            return false;
        }

        context.Notas.Remove(existente);
        context.SaveChanges();
        return true;
    }
}
