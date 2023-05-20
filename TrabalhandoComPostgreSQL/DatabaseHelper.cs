using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Npgsql;

namespace exercicio_bd
{
    class DatabaseHelper
    {
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=1973;Database=cliente_bd";
        private const string TABELA_NOME = "Cliente";
        private NpgsqlConnection npgsqlConnection;

        public DatabaseHelper()
        {
            npgsqlConnection = new NpgsqlConnection(CONNECTION_STRING);
        }

        public async Task AbrirConexao()
        {
            if (npgsqlConnection.State != ConnectionState.Open)
            {
                await npgsqlConnection.OpenAsync();
            }
        }

        public async Task FecharConexao()
        {
            if (npgsqlConnection.State != ConnectionState.Closed)
            {
                npgsqlConnection.Close();
            }
        }

        private Cliente DadosCliente(NpgsqlDataReader reader)
        {
            int id = Convert.ToInt32(reader["id"]);
            string nome = Convert.ToString(reader["nome"]);
            string endereco = Convert.ToString(reader["endereco"]);
            string telefone = Convert.ToString(reader["telefone"]);
            string email = Convert.ToString(reader["email"]);
            return new Cliente(id, nome, endereco, telefone, email);
        }

        public async Task CriarTabela()
        {
            {
                await AbrirConexao(); // Open the database connection

                try
                {
                    string commandText = $"CREATE TABLE IF NOT EXISTS {TABELA_NOME} (id serial PRIMARY KEY, nome VARCHAR(50), endereco VARCHAR(100), telefone VARCHAR(20), email VARCHAR(50))";
                    using (var cmd = new NpgsqlCommand(commandText, npgsqlConnection))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                finally
                {
                    await FecharConexao(); // Close the database connection
                }
            }
        }

        public async Task Cadastrar()
{
    Console.WriteLine("Informe os dados do cliente:");
    Cliente cliente = await InformarDadosCliente();

    await AbrirConexao(); // Abre a conexão com o banco de dados

    string commandText = $"INSERT INTO {TABELA_NOME} (nome, endereco, telefone, email) VALUES (@nome, @endereco, @telefone, @email)";
    using (var cmd = new NpgsqlCommand(commandText, npgsqlConnection))
    {
        cmd.Parameters.AddWithValue("nome", cliente.Nome);
        cmd.Parameters.AddWithValue("endereco", cliente.Endereco);
        cmd.Parameters.AddWithValue("telefone", cliente.Telefone);
        cmd.Parameters.AddWithValue("email", cliente.Email);

        await cmd.ExecuteNonQueryAsync();
        Console.WriteLine("Cliente cadastrado com sucesso!");
    }

    await FecharConexao(); // Fecha a conexão com o banco de dados
}

        public async Task Editar(int id)
{
    Cliente clienteExistente = await ListarPeloId(id);
    if (clienteExistente == null)
    {
        Console.WriteLine("Cliente não encontrado!");
        return;
    }

    Console.WriteLine("Informe os novos dados do cliente:");
    Cliente clienteAtualizado = await InformarDadosCliente();

    await AbrirConexao(); // Abre a conexão com o banco de dados

    string commandText = $"UPDATE {TABELA_NOME} SET nome = @nome, endereco = @endereco, telefone = @telefone, email = @email WHERE id = @id";
    using (var cmd = new NpgsqlCommand(commandText, npgsqlConnection))
    {
        cmd.Parameters.AddWithValue("nome", clienteAtualizado.Nome);
        cmd.Parameters.AddWithValue("endereco", clienteAtualizado.Endereco);
        cmd.Parameters.AddWithValue("telefone", clienteAtualizado.Telefone);
        cmd.Parameters.AddWithValue("email", clienteAtualizado.Email);
        cmd.Parameters.AddWithValue("id", id);

        await cmd.ExecuteNonQueryAsync();
        Console.WriteLine("Cliente atualizado com sucesso!");
    }

    await FecharConexao(); // Fecha a conexão com o banco de dados
}

        public async Task Excluir(int id)
{
    Cliente clienteExistente = await ListarPeloId(id);
    if (clienteExistente == null)
    {
        Console.WriteLine("Cliente não encontrado!");
        return;
    }

    await AbrirConexao(); // Abre a conexão com o banco de dados

    string commandText = $"DELETE FROM {TABELA_NOME} WHERE id = @id";
    using (var cmd = new NpgsqlCommand(commandText, npgsqlConnection))
    {
        cmd.Parameters.AddWithValue("id", id);

        await cmd.ExecuteNonQueryAsync();
        Console.WriteLine("Cliente excluído com sucesso!");
    }

    await FecharConexao(); // Fecha a conexão com o banco de dados
}

        public async Task<List<Cliente>> ListarTodos()
        {
            {
                await AbrirConexao(); // Open the database connection

                try
                {
                    List<Cliente> clientes = new List<Cliente>();

                    string commandText = $"SELECT * FROM {TABELA_NOME}";
                    using (var cmd = new NpgsqlCommand(commandText, npgsqlConnection))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Cliente cliente = DadosCliente(reader);
                                clientes.Add(cliente);
                            }
                        }
                    }

                    return clientes;
                }
                finally
                {
                    await FecharConexao(); // Close the database connection
                }
            }
        }

        public async Task<Cliente> ListarPeloId(int id)
{
    await AbrirConexao(); // Abre a conexão com o banco de dados

    string commandText = $"SELECT * FROM {TABELA_NOME} WHERE id = @id";
    using (var cmd = new NpgsqlCommand(commandText, npgsqlConnection))
    {
        cmd.Parameters.AddWithValue("id", id);

        using (var reader = await cmd.ExecuteReaderAsync())
        {
            if (await reader.ReadAsync())
            {
                Cliente cliente = DadosCliente(reader);
                await FecharConexao(); // Fecha a conexão com o banco de dados
                return cliente;
            }
        }
    }

    await FecharConexao(); // Fecha a conexão com o banco de dados
    return null;
}

        public async Task<Cliente> InformarDadosCliente()
        {
            Console.Write("Nome: ");
            string nome = Console.ReadLine();

            Console.Write("Endereço: ");
            string endereco = Console.ReadLine();

            Console.Write("Telefone: ");
            string telefone = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Cliente cliente = new Cliente(nome, endereco, telefone, email);
            return cliente;
        }
    }
}