using ContaCorrente.Domain.Entidade;
using ContaCorrente.Infrastructure.Interface;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ContaCorrente.Infrastructure.Repositories
{
    /// <summary>
    /// Cliente Repository
    /// </summary>
    public class ClienteRepository : IClienteRepository
    {
        const string _connectionString = "Data Source=DESKTOP-ST6PSQ7;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        /// <summary>
        /// Construtor da classe ClienteRepository
        /// </summary>        
        public ClienteRepository()
        {
        }

        /// <summary>
        /// Obter o ultimo identificador do banco de dados
        /// </summary>
        /// <returns></returns>
        public async Task<int> VerificaSeCpfJaEstaCadastrado(string idContaCorrente)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var existeConta = await connection.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM master.dbo.contacorrente WHERE idcontacorrente = @idcontacorrente", new { idcontacorrente = idContaCorrente });
            connection.Close();
            return existeConta;
        }

        /// <summary>
        /// Gravar dados do cliente no banco de dados
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public async Task GravarDadosCliente(Cliente cliente)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            await connection.ExecuteScalarAsync<int>("INSERT INTO contacorrente (idcontacorrente, numero, nome, ativo, senha, salt)" +
                                                     "VALUES (@idcontacorrente, @numero, @nome, @ativo, @senha, @salt)",
                                                     new
                                                     {
                                                         idcontacorrente = cliente.IdContaCorrente,
                                                         numero = cliente.NumeroConta,
                                                         nome = cliente.Nome,
                                                         ativo = cliente.Ativo,
                                                         senha = cliente.Senha,
                                                         salt = cliente.Salt
                                                     });
            connection.Close();
        }

        /// <summary>
        /// Obter Cliente por CPF
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public async Task<Cliente> ObterClientePorCpf(string cpf)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var cliente = await connection.QueryFirstOrDefaultAsync<Cliente>(
                "SELECT idcontacorrente as IdContaCorrente," +
                " numero as NumeroConta," +
                " nome as Nome," +
                " ativo as Ativo," +
                " senha as Senha," +
                " salt as Salt" +
                " FROM contacorrente WHERE idcontacorrente = @cpf", new { cpf });
            connection.Close();
            return cliente;
        }

        /// <summary>
        /// Gravar dados do cliente no banco de dados
        /// </summary>
        /// <param name="idContaCorrente"></param>
        /// <returns></returns>        
        public async Task InativarContaCliente(string idContaCorrente)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            await connection.ExecuteScalarAsync<int>("UPDATE contacorrente" +
                                                     "SET ativo = 0" +
                                                     "VALUES idcontacorrente = @idcontacorrente",
                                                     new { idcontacorrente = idContaCorrente });
            connection.Close();
        }
    }
}
