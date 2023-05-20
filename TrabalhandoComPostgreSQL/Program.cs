using System;
using static exercicio_bd.DatabaseHelper;

namespace exercicio_bd
{
    class Program
    {

        static async Task Main(string[] args)
        {
            Console.WriteLine("Bem-vindo!");
            DatabaseHelper databaseHelper = new DatabaseHelper();

            int opcao;
            int id;

            do
            {
                await databaseHelper.CriarTabela();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n =========== MENU =========== ");
                Console.ResetColor();
                Console.WriteLine("1 - Cadastrar cliente");
                Console.WriteLine("2 - Atualizar cliente existente");
                Console.WriteLine("3 - Excluir cliente");
                Console.WriteLine("4 - Listar clientes");
                Console.WriteLine("5 - Pesquisar dados de cliente");
                Console.WriteLine("0 - Sair");
                Console.Write("\nDigite a opção desejada: ");
                opcao = Convert.ToInt32(Console.ReadLine());

                switch (opcao)
                {
                    case 1: // Cadastrar
                        await databaseHelper.Cadastrar();
                        break;
                    case 2: // Editar
                        Console.WriteLine("Informe o ID do cliente que deseja Editar: ");
                        id = Convert.ToInt32(Console.ReadLine());
                        await databaseHelper.Editar(id);
                        break;
                    case 3: // Excluir
                        Console.WriteLine("Informe o ID do cliente que deseja Excluir: ");
                        id = Convert.ToInt32(Console.ReadLine());
                        await databaseHelper.Excluir(id);
                        break;
                    case 4: // Listar Todos
                        List<Cliente> clientes = await databaseHelper.ListarTodos();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\n=== Lista de clientes ===");
                        Console.ResetColor();
                        foreach (Cliente cliente in clientes)
                        {
                            Console.WriteLine($"\nId: {cliente.Id} | Nome: {cliente.Nome} | Endereço: {cliente.Endereco} | Telefone: {cliente.Telefone} | E-mail: {cliente.Email}");
                        }
                        break;
                    case 5: // Pesquisar por ID
                        Console.WriteLine("Informe o ID do cliente que deseja pesquisar: ");
                        id = Convert.ToInt32(Console.ReadLine());
                        Cliente clienteEncontrado = await databaseHelper.ListarPeloId(id);
                        if (clienteEncontrado != null)
                        {
                            Console.WriteLine("Cliente encontrado:");
                            Console.WriteLine($"ID: {clienteEncontrado.Id}");
                            Console.WriteLine($"Nome: {clienteEncontrado.Nome}");
                            Console.WriteLine($"Endereço: {clienteEncontrado.Endereco}");
                            Console.WriteLine($"Telefone: {clienteEncontrado.Telefone}");
                            Console.WriteLine($"E-mail: {clienteEncontrado.Email}");
                        }
                        else
                        {
                            Console.WriteLine("Cliente não encontrado!");
                        }
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }

                databaseHelper.FecharConexao();
            } while (opcao != 0);
        }
    }
}