using MeuDiarioSENAC.Model;

namespace MeuDiarioSENAC.Business;

public enum ResultadoEntrada
{
	TentarNovamente,
	VoltarAoMenu,
	Sair
}

public class RegistroBusiness
{
	public ResultadoEntrada EscolherAposEntradaInvalida()
	{
		while (true)
		{
			Console.WriteLine("1 - Entrar novamente com os dados");
			Console.WriteLine("2 - Voltar ao Menu Principal");
			Console.WriteLine("3 - Sair do Programa");
			Console.Write("Escolha uma opção: ");

			switch (Console.ReadLine())
			{
				case "1": return ResultadoEntrada.TentarNovamente;
				case "2": return ResultadoEntrada.VoltarAoMenu;
				case "3": return ResultadoEntrada.Sair;
				default: Console.WriteLine("Opção inválida. Escolha 1, 2 ou 3."); break;
			}
		}
	}

	public bool ValidarTitulo(string? titulo)
	{
		try
		{
			return !string.IsNullOrWhiteSpace(titulo) && titulo.Length <= 50;
		}
		catch
		{
			return false;
		}
	}

	public bool ValidarData(DateTime data)
	{
		try
		{
			return data.Date == DateTime.Today;
		}
		catch
		{
			return false;
		}
	}

	public bool ValidarConteudo(string? conteudo)
	{
		try
		{
			return !string.IsNullOrWhiteSpace(conteudo) && conteudo.Length <= 3000;
		}
		catch
		{
			return false;
		}
	}

	public bool ValidarNota(Nota nota)
	{
		try
		{
			if (!ValidarTitulo(nota.Titulo))
			{
				throw new ArgumentException("O título é obrigatório e deve ter no máximo 50 caracteres.");
			}

			if (!ValidarData(nota.Data))
			{
				throw new ArgumentException("A data deve ser a data atual.");
			}

			if (!ValidarConteudo(nota.Conteudo))
			{
				throw new ArgumentException("O conteúdo deve ter no máximo 3000 caracteres.");
			}

			return true;
		}
		catch (ArgumentException)
		{
			throw;
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException("Não foi possível validar a nota.", ex);
		}
	}
}
