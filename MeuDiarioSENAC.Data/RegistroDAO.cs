using Microsoft.EntityFrameworkCore;
using MeuDiarioSENAC.Business;
using MeuDiarioSENAC.Model;

namespace MeuDiarioSENAC.Data;

public class RegistroDAO
{
    private readonly MeuDiarioSENACContext _context;
    private readonly RegistroBusiness _registroBusiness;

    public RegistroDAO(MeuDiarioSENACContext context, RegistroBusiness registroBusiness)
    {
        _context = context;
        _registroBusiness = registroBusiness;
    }

    public bool CriarRegistro()
    {
        while (true)
        {
            Console.Write("Digite o Título: ");
            string titulo = Console.ReadLine() ?? string.Empty;

            if (!_registroBusiness.ValidarTitulo(titulo))
            {
                Console.WriteLine("Dados inválidos: o título é obrigatório e deve ter no máximo 50 caracteres.");
                ResultadoEntrada resultado = _registroBusiness.EscolherAposEntradaInvalida();
                if (resultado == ResultadoEntrada.TentarNovamente) continue;
                return resultado == ResultadoEntrada.VoltarAoMenu;
            }

            Console.Write("Digite o Conteúdo: ");
            string conteudo = Console.ReadLine() ?? string.Empty;

            if (!_registroBusiness.ValidarConteudo(conteudo))
            {
                Console.WriteLine("Dados inválidos: o conteúdo é obrigatório e deve ter no máximo 3000 caracteres.");
                ResultadoEntrada resultado = _registroBusiness.EscolherAposEntradaInvalida();
                if (resultado == ResultadoEntrada.TentarNovamente) continue;
                return resultado == ResultadoEntrada.VoltarAoMenu;
            }

            var novaNota = new Nota(titulo, conteudo);

            try
            {
                _registroBusiness.ValidarNota(novaNota);
                _context.Notas.Add(novaNota);
                _context.SaveChanges();
                Console.WriteLine("Registro criado com sucesso!");
                Console.WriteLine("Voltando ao menu principal...");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar o registro: {ex.Message}");
                Console.WriteLine("Verifique se o MySQL está em execução e se as credenciais do banco estão corretas.");
                return true;
            }
        }
    }

    public void ListarRegistros()
    {
        var registros = _context.Notas.AsNoTracking().OrderBy(n => n.Id).ToList();

        if (registros.Count == 0)
        {
            Console.WriteLine("Nenhum registro encontrado.");
            return;
        }

        foreach (var nota in registros)
        {
            Console.WriteLine($"\nID: {nota.Id}");
            Console.WriteLine($"Título: {nota.Titulo}");
            Console.WriteLine($"Data: {nota.Data:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Conteúdo: {nota.Conteudo}");
        }

        Console.WriteLine("\nVoltando ao menu principal...");
    }

    public bool BuscarRegistroPorId()
    {
        Console.Write("Digite o ID do registro: ");
        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
        {
            Console.WriteLine("Dados inválidos: informe um ID numérico maior que zero.");
            ResultadoEntrada resultado = _registroBusiness.EscolherAposEntradaInvalida();
            if (resultado == ResultadoEntrada.TentarNovamente) return BuscarRegistroPorId();
            return resultado == ResultadoEntrada.VoltarAoMenu;
        }

        var nota = _context.Notas.Find(id);

        if (nota is null)
        {
            Console.WriteLine("Registro não encontrado.");
            return true;
        }

        Console.WriteLine($"\nID: {nota.Id}");
        Console.WriteLine($"Título: {nota.Titulo}");
        Console.WriteLine($"Data: {nota.Data:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Conteúdo: {nota.Conteudo}");
        Console.WriteLine("\nVoltando ao menu principal...");
        return true;
    }

    public bool DeletarRegistroPorId()
    {
        Console.Write("Digite o ID do registro que deseja deletar: ");
        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
        {
            Console.WriteLine("Dados inválidos: informe um ID numérico maior que zero.");
            ResultadoEntrada resultado = _registroBusiness.EscolherAposEntradaInvalida();
            if (resultado == ResultadoEntrada.TentarNovamente) return DeletarRegistroPorId();
            return resultado == ResultadoEntrada.VoltarAoMenu;
        }

        var nota = _context.Notas.Find(id);

        if (nota is null)
        {
            Console.WriteLine("Registro não encontrado.");
            return true;
        }

        try
        {
            _context.Notas.Remove(nota);
            _context.SaveChanges();

            Console.WriteLine("Registro deletado com sucesso!");
            Console.WriteLine("\nVoltando ao menu principal...");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao excluir o registro: {ex.Message}");
            Console.WriteLine("Verifique se o MySQL está em execução e se as credenciais do banco estão corretas.");
            return true;
        }
    }
}