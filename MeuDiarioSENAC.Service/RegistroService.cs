using MeuDiarioSENAC.Business;
using MeuDiarioSENAC.Data.Repositories;
using MeuDiarioSENAC.Model;

namespace MeuDiarioSENAC.Service;

public class RegistroService
{
    private readonly NotaRepository _repository;
    private readonly RegistroBusiness _business;

    public RegistroService(NotaRepository repository, RegistroBusiness business)
    {
        _repository = repository;
        _business = business;
    }

    public List<Nota> Listar(int? id = null, string? titulo = null, DateTime? data = null, string? conteudo = null)
    {
        if (id.HasValue && id.Value <= 0)
        {
            throw new ArgumentException("O ID deve ser maior que zero.");
        }

        return _repository.Listar(id, titulo, data, conteudo);
    }

    public Nota Criar(string titulo, string conteudo)
    {
        var nota = new Nota(titulo.Trim(), conteudo.Trim());
        _business.ValidarNota(nota);
        return _repository.Adicionar(nota);
    }

    public Nota? BuscarPorId(int id)
    {
        return _repository.BuscarPorId(id);
    }

    public bool Atualizar(int id, string titulo, string conteudo)
    {
        if (!_business.ValidarTitulo(titulo))
        {
            throw new ArgumentException("O título é obrigatório e deve ter no máximo 50 caracteres.");
        }

        if (!_business.ValidarConteudo(conteudo))
        {
            throw new ArgumentException("O conteúdo é obrigatório e deve ter no máximo 3000 caracteres.");
        }

        return _repository.Atualizar(new Nota
        {
            Id = id,
            Titulo = titulo.Trim(),
            Conteudo = conteudo.Trim()
        });
    }

    public bool Excluir(int id)
    {
        return _repository.Excluir(id);
    }
}
