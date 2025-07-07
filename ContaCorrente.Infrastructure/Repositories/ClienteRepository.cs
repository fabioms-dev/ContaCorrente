using ContaCorrente.Domain.Entidade;
using ContaCorrente.Infrastructure.Interface;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ContaCorrente.Infrastructure.Repositories
{
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
                                                     "VALUES (@idcontacorrente, @numero, @nome, @ativo, @senha, @salt);",
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
    }
}
