using MeuDiarioSENAC.Data;

namespace DiarioPessoalApp
{
    class DiarioPessoal
    {
        private readonly MeuDiarioSENACContext db = new();
        private readonly RegistroDAO dao;

        public DiarioPessoal()
        {
            dao = new RegistroDAO(db);
            db.GarantirBancoETabela();
        }

        public void Executar()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n=== Diário Pessoal ===");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("1 - Criar novo registro");
                Console.WriteLine("2 - Listar todos os registros");
                Console.WriteLine("3 - Buscar registro por ID");
                Console.WriteLine("4 - Deletar registro por ID");
                Console.WriteLine("5 - Sair\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Escolha uma opção: ");
                Console.ForegroundColor = ConsoleColor.White;

                string? opcao = Console.ReadLine();
                Console.WriteLine("");

                switch (opcao)
                {
                    case "1": dao.CriarRegistro(); break;
                    case "2": dao.ListarRegistros(); break;
                    case "3": dao.BuscarRegistroPorId(); break;
                    case "4": dao.DeletarRegistroPorId(); break;
                    case "5": return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        RegistroDAO.ContinuarAposErro("Escolha uma ação para continuar:");
                        break;
                }
            }
        }
    }
}