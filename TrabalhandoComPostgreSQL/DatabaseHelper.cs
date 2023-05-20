using Npgsql;

namespace exercicio_bd {
    public class DatabaseHelper {
        private NpgsqlConnection npgsqlConnection;
        private const string TABELA_NOME = "cliente";
        public DatabaseHelper() {
            var builder = new NpgsqlConnectionStringBuilder();
            builder.Host = "localhost";
            builder.Port = 5432;
            builder.Username = "postgres";
            builder.Password = "1973";
            builder.Database = "cliente_bd";
            npgsqlConnection = new NpgsqlConnection(builder.ConnectionString);
            npgsqlConnection.Open();
        }

        public void fecharConexao() {
            npgsqlConnection.Close();
        }

        public async Task criarTabela() { // somente se ainda não existir
            var comandoSql = $"CREATE TABLE IF NOT EXISTS {TABELA_NOME}" +
                $"(" +
                    $"id serial PRIMARY KEY, " +
                    $"nome VARCHAR (100), " +
                    $"endereco VARCHAR (100), " +
                    $"telefone VARCHAR (15), " +
                    $"email VARCHAR (255)" +
                $");";
            using var cmd = new NpgsqlCommand(comandoSql, npgsqlConnection);
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task insertOrUpdateOrDeleteCliente (string comandoSql, int id, bool deletar) {
            Cliente cliente = new Cliente();
            if (!deletar) { // para a adição e edição o usuário informa todos os dados do cliente.
                cliente = informarDadosCliente();
            }
            await using (var param = new NpgsqlCommand(comandoSql, npgsqlConnection)) {
                if (id != 0) {
                    param.Parameters.AddWithValue("id", id);
                }
                if (cliente.nome != null) {
                    param.Parameters.AddWithValue("nome", cliente.nome);
                }
                if (cliente.endereco != null) {
                    param.Parameters.AddWithValue("endereco", cliente.endereco);
                }
                if (cliente.telefone != null) {
                    param.Parameters.AddWithValue("telefone", cliente.telefone);
                }
                if (cliente.email != null) {
                    param.Parameters.AddWithValue("email", cliente.email);
                }
                await param.ExecuteNonQueryAsync();
            }
        }

        public async Task cadastrar() {
            string comandoSql = $"INSERT INTO {TABELA_NOME} (nome, endereco, telefone, email) VALUES (@nome, @endereco, @telefone, @email)";
            await insertOrUpdateOrDeleteCliente(comandoSql, 0, false);
            Console.WriteLine("Cliente cadastrado!");
        }

        public async Task editar(int id) {
            if (listarPeloId(id).Result == null) {
                Console.WriteLine("ID do cliente não cadastrado!");
            } else {
                var comandoSql = $"UPDATE {TABELA_NOME} SET nome = @nome, endereco = @endereco, telefone = @telefone, email = @email WHERE id = @id";
                await insertOrUpdateOrDeleteCliente(comandoSql, id, false);
                Console.WriteLine("Cliente atualizado!");
            }
        }

        public async Task excluir(int id) {
            if (listarPeloId(id).Result == null) {
                Console.WriteLine("ID do cliente não cadastrado!");
            } else {
                string comandoSql = $"DELETE FROM {TABELA_NOME} WHERE id=@id";
                await insertOrUpdateOrDeleteCliente(comandoSql, id, true);
                Console.WriteLine("Cliente excluído!");
            }
        }

        public async Task<Cliente?> listarPeloId(int id) {
            string commandText = $"SELECT * FROM {TABELA_NOME} WHERE id = @id";
            await using (var param = new NpgsqlCommand(commandText, npgsqlConnection)) {
                param.Parameters.AddWithValue("id", id);
                await using (var reader = await param.ExecuteReaderAsync())
                while (await reader.ReadAsync()) {
                    Cliente cliente = dadosCliente(reader);
                    return cliente;
                }
            }
            return null;
        }

        public async Task<List<Cliente>> listarTodos() {
            string commandText = $"SELECT * FROM {TABELA_NOME} ORDER BY id";
            await using (var param = new NpgsqlCommand(commandText, npgsqlConnection)) {
                List<Cliente> clientes = new List<Cliente>();
                await using (var reader = await param.ExecuteReaderAsync())
                while (await reader.ReadAsync()) {
                    clientes.Add(dadosCliente(reader));
                }
                return clientes;
            }
        }

        private static Cliente dadosCliente(NpgsqlDataReader reader) {
            int? id = reader["id"] as int?;
            string? nome = reader["nome"] as string;
            string? endereco = reader["endereco"] as string;
            string? telefone = reader["telefone"] as string;
            string? email = reader["email"] as string;

            Cliente cliente = new Cliente();
            if (id != null) {
                cliente.id = id.Value;
            }
            cliente.nome = nome;
            cliente.endereco = endereco;
            cliente.telefone = telefone;
            cliente.email = email;
            return cliente;
        }

        private Cliente informarDadosCliente () {
            Cliente cliente = new Cliente();
            Console.WriteLine("Informe o nome: ");
            cliente.nome = Console.ReadLine();
            Console.WriteLine("Informe o endereço: ");
            cliente.endereco = Console.ReadLine();
            Console.WriteLine("Informe o telefone: ");
            cliente.telefone = Console.ReadLine();
            bool emailValido = false;
            string? email = "";
            while (!emailValido) {
                Console.WriteLine("Informe o e-mail: ");
                email = Console.ReadLine();
                emailValido = validarEmail(email);
                if (!emailValido) {
                    Console.WriteLine("E-mail inválido, informe novamente!");
                }
            }
            cliente.email = email;
            return cliente;
        }

        private bool validarEmail (string? email) {
            if (email == null) {
                return false;
            }
            var trimEmail = email.Trim();
            if (trimEmail.EndsWith(".")) {
                return false;
            }
            try {
                var endereco = new System.Net.Mail.MailAddress(email);
                return endereco.Address == trimEmail;
            }
            catch {
                return false;
            }
        }
    }
}