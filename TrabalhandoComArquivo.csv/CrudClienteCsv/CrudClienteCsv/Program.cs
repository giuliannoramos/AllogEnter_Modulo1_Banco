using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudClienteCsv
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Cliente> listaClientes = new List<Cliente>();

            int opcao;
            do
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n =========== MENU =========== ");
                Console.ResetColor();
                Console.WriteLine("1 - Cadastrar cliente");
                Console.WriteLine("2 - Atualizar cliente existente");
                Console.WriteLine("3 - Excluir cliente");
                Console.WriteLine("4 - Listar clientes");
                Console.WriteLine("5 - Pesquisar dados de cliente");
                Console.WriteLine("0 - Sair");
                opcao = Cliente.ValidarInteiro("\nDigite a opção desejada: ");

                switch (opcao)
                {
                    case 1:
                        Cliente.CadastrarClienteCsv(listaClientes);
                        break;
                    case 2:
                        Cliente.AtualizarClienteCsv(listaClientes);
                        break;
                    case 3:
                        Cliente.ExcluirClienteCsv(listaClientes);
                        break;
                    case 4:
                        Cliente.ListaCompletaCsv();
                        break;
                    case 5:
                        Cliente.PesquisarClientesCsv(listaClientes);
                        break;
                    case 0:
                        Console.WriteLine("\nPrograma encerrado.");
                        break;
                    default:
                        Console.WriteLine("\nOpção inválida!");
                        break;
                }

            } while (opcao != 0);
        }
    }
}
