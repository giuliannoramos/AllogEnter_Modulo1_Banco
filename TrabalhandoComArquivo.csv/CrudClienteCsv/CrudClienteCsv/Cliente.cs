using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudClienteCsv
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }

        public Cliente(int id, string nome, string endereco, string telefone, string email)
        {
            Id = id;
            Nome = nome;
            Endereco = endereco;
            Telefone = telefone;
            Email = email;
        }

        public static void CadastrarClienteCsv(List<Cliente> listaClientes)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n=== Cadastrar cliente ===");
            Console.ResetColor();
            Console.WriteLine("\nPara cadastrar, preencha os dados abaixo:");

            // Carrega a lista de clientes existentes no arquivo CSV
            var clientesCSV = CarregarClientesCsv();

            // Verifica o maior ID atual na lista de clientes
            int maxId = clientesCSV.Any() ? clientesCSV.Max(c => c.Id) : 0;

            // Gera um novo ID automático
            int id = maxId + 1;

            // Solicita os dados do novo cliente
            string nome = ValidarString("Nome: ");
            nome = nome.Replace(",", ";");
            string endereco = ValidarString("Endereço: ");
            endereco = endereco.Replace(",", ";");
            string telefone = ValidarString("Telefone: ");
            telefone = telefone.Replace(",", ";");
            string email = ValidarString("Email: ");
            while (!ValidarEmail(email))
            {
                Console.WriteLine("\nO email digitado é inválido! Digite novamente.");
                email = ValidarString("Email: ");
            }
            email = email.Replace(",", ";");

            // Cria o objeto Cliente com os dados do novo cliente
            var novoCliente = new Cliente(
                id,
                nome,
                endereco,
                telefone,
                email
            );

            // Adiciona o novo cliente na lista e salva no arquivo CSV
            listaClientes.Add(novoCliente);
            using (var writer = new StreamWriter("clientes.csv", false))
            {
                foreach (var cliente in listaClientes)
                {
                    writer.WriteLine($"{cliente.Id},{cliente.Nome},{cliente.Endereco},{cliente.Telefone},{cliente.Email}");
                }
            }

            Console.WriteLine("\nCliente adicionado com sucesso!");
        }


        public static void AtualizarClienteCsv(List<Cliente> listaClientes)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n=== Atualizar cliente ===");
            Console.ResetColor();

            // carrega a lista de clientes existentes no arquivo CSV
            var clientesCSV = CarregarClientesCsv();

            // solicita o ID do cliente a ser atualizado
            int idCliente = ValidarInteiro("\nDigite o ID do cliente que deseja atualizar: ");

            // encontra o cliente a ser atualizado
            var clienteAntigo = clientesCSV.FirstOrDefault(c => c.Id == idCliente);
            if (clienteAntigo == null)
            {
                Console.WriteLine("\nCliente não encontrado!");
                return;
            }

            Console.WriteLine($"\nVocê está prestes a alterar os dados do cliente {clienteAntigo.Id}.");
            Console.WriteLine("Para manter os dados antigos, pressione ENTER sem digitar nada.");

            // cria o objeto Cliente com os dados atualizados
            var clienteAtualizado = new Cliente(
                idCliente,
                ValidarStringAtualizando($"\nDigite o novo nome (ou aperte enter para manter '{clienteAntigo.Nome}'): ", clienteAntigo.Nome).Replace(",", ";"),
                ValidarStringAtualizando($"Digite o novo endereço (ou aperte enter para manter '{clienteAntigo.Endereco}'): ", clienteAntigo.Endereco).Replace(",", ";"),
                ValidarStringAtualizando($"Digite o novo telefone (ou aperte enter para manter '{clienteAntigo.Telefone}'): ", clienteAntigo.Telefone).Replace(",", ";"),
                ValidarStringAtualizando($"Digite o novo email (ou aperte enter para manter '{clienteAntigo.Email}'): ", clienteAntigo.Email).Replace(",", ";")
            );


            // atualiza a lista de clientes
            var indexCliente = clientesCSV.IndexOf(clienteAntigo);
            clientesCSV[indexCliente] = clienteAtualizado;

            // salva a lista atualizada no arquivo CSV
            using (var writer = new StreamWriter("clientes.csv", false))
            {
                foreach (var cliente in clientesCSV)
                {
                    writer.WriteLine($"{cliente.Id},{cliente.Nome},{cliente.Endereco},{cliente.Telefone},{cliente.Email}");
                }
            }

            Console.WriteLine("\nCliente atualizado com sucesso!");
        }

        public static void ExcluirClienteCsv(List<Cliente> listaClientes)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n=== Excluir cliente ===");
            Console.ResetColor();

            // carrega a lista de clientes existentes no arquivo CSV
            var clientesCSV = CarregarClientesCsv();

            // solicita o ID do cliente a ser excluído
            int idCliente = ValidarInteiro("\nDigite o ID do cliente que deseja excluir: ");

            // encontra o cliente a ser excluído
            var clienteExcluir = clientesCSV.FirstOrDefault(c => c.Id == idCliente);
            if (clienteExcluir == null)
            {
                Console.WriteLine("\nCliente não encontrado!");
                return;
            }

            // remove o cliente da lista
            clientesCSV.Remove(clienteExcluir);

            // salva a lista atualizada no arquivo CSV
            using (var writer = new StreamWriter("clientes.csv", false))
            {
                foreach (var cliente in clientesCSV)
                {
                    writer.WriteLine($"{cliente.Id},{cliente.Nome},{cliente.Endereco},{cliente.Telefone},{cliente.Email}");
                }
            }

            Console.WriteLine("\nCliente excluído com sucesso!");
        }

        public static void ListaCompletaCsv()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n=== Lista de clientes ===");
            Console.ResetColor();

            // carrega a lista de clientes existentes no arquivo CSV
            var clientesCSV = CarregarClientesCsv();

            // verifica se existem clientes cadastrados
            if (clientesCSV.Count == 0)
            {
                Console.WriteLine("\nNão existem clientes cadastrados!");
                return;
            }

            // imprime a lista completa de clientes            
            foreach (var cliente in clientesCSV)
            {
                Console.WriteLine($"\nId: {cliente.Id} | Nome: {cliente.Nome} | Endereço: {cliente.Endereco} | Telefone: {cliente.Telefone} | E-mail: {cliente.Email}");
            }
        }

        public static void PesquisarClientesCsv(List<Cliente> listaClientes)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n=== Pesquisar por cliente ===");
            Console.ResetColor();

            Console.WriteLine("\nDigite um dado para efetuar a busca:");
            var valorBusca = Console.ReadLine();

            var encontrados = false;

            using (var reader = new StreamReader("clientes.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var linha = reader.ReadLine();
                    var campos = linha.Split(',');

                    if (campos[0].Equals(valorBusca) ||
                        campos[1].Equals(valorBusca, StringComparison.OrdinalIgnoreCase) ||
                        campos[2].Equals(valorBusca, StringComparison.OrdinalIgnoreCase) ||
                        campos[3].Equals(valorBusca) ||
                        campos[4].Equals(valorBusca, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("\nCliente encontrado:");
                        Console.WriteLine($"ID: {campos[0]}");
                        Console.WriteLine($"Nome: {campos[1]}");
                        Console.WriteLine($"Endereço: {campos[2]}");
                        Console.WriteLine($"Telefone: {campos[3]}");
                        Console.WriteLine($"E-mail: {campos[4]}");
                        encontrados = true;
                        break;
                    }
                }
            }

            if (!encontrados)
            {
                Console.WriteLine("Nenhum cliente encontrado.");
            }

        }

        public static string ValidarString(string mensagem)
        {
            string entrada = null;
            while (entrada == null)
            {
                try
                {
                    Console.Write(mensagem);
                    entrada = Console.ReadLine();
                    if (entrada == null)
                    {
                        throw new Exception("\nEntrada inválida, por favor digite novamente.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return entrada;
        }

        public static int ValidarInteiro(string mensagem)
        {
            int numero;
            while (true)
            {
                Console.Write(mensagem);
                try
                {
                    numero = int.Parse(Console.ReadLine());
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nValor inválido. Digite apenas números inteiros.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("\nValor inválido. O número digitado é muito grande.");
                }
            }
            return numero;
        }

        public static bool ValidarEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static List<Cliente> CarregarClientesCsv()
        {
            var clientesCSV = new List<Cliente>();
            using (var reader = new StreamReader("clientes.csv"))
            {
                string linha;
                while ((linha = reader.ReadLine()) != null)
                {
                    var campos = linha.Split(',');
                    var client = new Cliente(
                        int.Parse(campos[0]),
                        campos[1],
                        campos[2],
                        campos[3],
                        campos[4]
                    );
                    clientesCSV.Add(client);
                }
            }

            if (clientesCSV.Count == 0)
            {
                Console.WriteLine("Não foram encontrados clientes na lista.");
            }

            return clientesCSV;
        }

        public static string ValidarStringAtualizando(string mensagem, string padrao = "")
        {
            Console.Write(mensagem);
            string valor = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(valor) && !string.IsNullOrEmpty(padrao))
            {
                return padrao;
            }

            if (mensagem.ToLower().Contains("email") && !ValidarEmail(valor))
            {
                Console.WriteLine("E-mail inválido. Digite novamente.");
                return ValidarStringAtualizando(mensagem, padrao);
            }

            return valor;
        }

    }
}
