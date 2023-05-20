using static exercicio_bd.DatabaseHelper;

namespace exercicio_bd
{
    class Program
    {

        static async Task Main(string[] args)
        {            
            DatabaseHelper databaseHelper = new DatabaseHelper();
            databaseHelper.fecharConexao();
            await databaseHelper.criarTabela();

            int opcao;
            int id;
            
            do
            {
                Console.WriteLine("Bem-vindo!");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n =========== MENU =========== ");
                Console.ResetColor();
                Console.WriteLine("1 - Cadastrar cliente");
                Console.WriteLine("2 - Atualizar cliente existente");
                Console.WriteLine("3 - Excluir cliente");
                Console.WriteLine("4 - Listar clientes");
                //Console.WriteLine("5 - Pesquisar dados de cliente");
                Console.WriteLine("0 - Sair");
                Console.Write("\nDigite a opção desejada: ");
                opcao = Convert.ToInt32(Console.ReadLine());

                switch (opcao)
                {
                    case 1: //Cadastrar
                        await databaseHelper.cadastrar();
                        break;
                    case 2: //Editar
                        Console.WriteLine("Informe o ID do cliente que deseja Editar: ");
                        id = Convert.ToInt32(Console.ReadLine());
                        await databaseHelper.editar(id);
                        break;
                    case 3: //Excluir
                        Console.WriteLine("Informe o ID do cliente que deseja Excluir: ");
                        id = Convert.ToInt32(Console.ReadLine());
                        await databaseHelper.excluir(id);
                        break;
                    case 4: //Listar Todos
                        List<Cliente> clientes = await databaseHelper.listarTodos();
                        Console.WriteLine("ID \t Nome \t\t\t Endereço \t\t\t Telefone \t\t E-mail");
                        foreach (Cliente cliente in clientes)
                        {
                            Console.WriteLine(cliente.id + " \t " + cliente.nome + " \t\t\t " + cliente.endereco + " \t\t\t " + cliente.telefone + " \t\t " + cliente.email);
                        }
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
                databaseHelper.fecharConexao();

            } while (opcao != 0);

        }
    }
}