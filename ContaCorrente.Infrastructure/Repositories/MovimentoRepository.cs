using ContaCorrente.Domain.Dto;
using ContaCorrente.Infrastructure.Interface;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ContaCorrente.Infrastructure.Repositories
{
    /// <summary>
    /// Movimento de conta corrente repository
    /// </summary>
    public class MovimentoRepository : IMovimentoRepository
    {
        const string _connectionString = "Data Source=DESKTOP-ST6PSQ7;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        /// <summary>
        /// Construtor
        /// </summary>
        public MovimentoRepository() { }

        /// <summary>
        /// Obter o ultimo identificador do banco de dados
        /// </summary>
        /// <returns></returns>        
        public async Task<int> VerificaUltimoIdentificadorMovimento()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var ultimoIdentificadorMovimento = await connection.ExecuteScalarAsync<int?>("SELECT MAX(idmovimento) FROM movimento");
            connection.Close();
            return ultimoIdentificadorMovimento ?? 0;
        }

        /// <summary>
        /// Gravar movimento de conta corrente
        /// </summary>
        /// <param name="movimentacaoContaDto"></param>
        /// <returns></returns>
        public async Task GravarMovimentoContaCorrente(MovimentacaoContaDto movimentacaoContaDto)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            await connection.ExecuteScalarAsync<int>("INSERT INTO movimento (idmovimento, idcontacorrente, tipo, valor, datahora) " +
                                                     "VALUES (@idmovimento, @idcontacorrente, @tipo, @valor, @datahora)",
                                                     new
                                                     {
                                                         idmovimento = movimentacaoContaDto.idContaCorrente,
                                                         idcontacorrente = movimentacaoContaDto.idContaCorrente,
                                                         tipo = movimentacaoContaDto.TipoMovimento,
                                                         valor = movimentacaoContaDto.Valor,
                                                         datahora = DateTime.Now
                                                     });
            connection.Close();
        }
    }
}
