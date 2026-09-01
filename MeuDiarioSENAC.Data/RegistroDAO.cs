using Microsoft.EntityFrameworkCore;

namespace MeuDiarioSENAC.Data;

public class RegistroDAO
{
    private readonly MeuDiarioSENACContext _context;

    public RegistroDAO(MeuDiarioSENACContext context)
    {
        _context = context;
    }

    public static bool ContinuarAposErro(string mensagem)
    {
        while (true)
        {
            Console.WriteLine($"\n{mensagem}");
            Console.WriteLine("1 - Digitar um novo valor");
            Console.WriteLine("2 - Voltar ao menu principal");
            Console.WriteLine("3 - Sair do aplicativo");
            Console.Write("Escolha uma opção: ");

            string? opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    return true;
                case "2":
                    return false;
                case "3":
                    Environment.Exit(0);
                    return false;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }

    public void CriarRegistro()
    {
        while (true)
        {
            Console.Write("Digite o Título: ");
            string? titulo = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(titulo))
            {
                while (true)
                {
                    Console.Write("Digite o Conteúdo: ");
                    string? conteudo = Console.ReadLine()?.Trim();

                    if (!string.IsNullOrWhiteSpace(conteudo))
                    {
                        var novaNota = new Nota(titulo, conteudo);

                        try
                        {
                            _context.Notas.Add(novaNota);
                            _context.SaveChanges();
                            Console.WriteLine("Registro criado com sucesso!");
                            Console.WriteLine("Voltando ao menu principal...");
                            return;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao salvar o registro: {ex.Message}");
                            Console.WriteLine("Verifique se o MySQL está em execução e se as credenciais do banco estão corretas.");
                            return;
                        }
                    }

                    Console.WriteLine("O conteúdo não pode ser vazio.");
                    if (ContinuarAposErro("Deseja informar um novo conteúdo?"))
                    {
                        continue;
                    }

                    return;
                }
            }

            Console.WriteLine("O título não pode ser vazio.");
            if (ContinuarAposErro("Deseja informar um novo título?"))
            {
                continue;
            }

            return;
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

    public void BuscarRegistroPorId()
    {
        while (true)
        {
            Console.Write("Digite o ID do registro: ");
            string? entrada = Console.ReadLine();

            if (int.TryParse(entrada, out int id))
            {
                var nota = _context.Notas.Find(id);

                if (nota is null)
                {
                    Console.WriteLine("Registro não encontrado.");
                    if (ContinuarAposErro("Deseja tentar outro ID?"))
                    {
                        continue;
                    }

                    return;
                }

                Console.WriteLine($"\nID: {nota.Id}");
                Console.WriteLine($"Título: {nota.Titulo}");
                Console.WriteLine($"Data: {nota.Data:dd/MM/yyyy HH:mm}");
                Console.WriteLine($"Conteúdo: {nota.Conteudo}");
                Console.WriteLine("\nVoltando ao menu principal...");
                return;
            }

            Console.WriteLine("ID inválido.");
            if (ContinuarAposErro("Deseja digitar um novo ID?"))
            {
                continue;
            }

            return;
        }
    }

    public void DeletarRegistroPorId()
    {
        while (true)
        {
            Console.Write("Digite o ID do registro que deseja deletar: ");
            string? entrada = Console.ReadLine();

            if (int.TryParse(entrada, out int id))
            {
                var nota = _context.Notas.Find(id);

                if (nota is null)
                {
                    Console.WriteLine("Registro não encontrado.");
                    if (ContinuarAposErro("Deseja tentar outro ID para exclusão?"))
                    {
                        continue;
                    }

                    return;
                }

                try
                {
                    _context.Notas.Remove(nota);
                    _context.SaveChanges();

                    Console.WriteLine("Registro deletado com sucesso!");
                    Console.WriteLine("\nVoltando ao menu principal...");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao excluir o registro: {ex.Message}");
                    Console.WriteLine("Verifique se o MySQL está em execução e se as credenciais do banco estão corretas.");
                    return;
                }
            }

            Console.WriteLine("ID inválido.");
            if (ContinuarAposErro("Deseja digitar um novo ID para exclusão?"))
            {
                continue;
            }

            return;
        }
    }
}