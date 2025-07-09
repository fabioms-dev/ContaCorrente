using ContaCorrente.Domain.Dto;

namespace ContaCorrente.Infrastructure.Interface
{
    /// <summary>
    /// Movimento Repository Interface
    /// </summary>
    public interface IMovimentoRepository
    {
        /// <summary>
        /// Obter o ultimo identificador do banco de dados
        /// </summary>
        /// <returns></returns>        
        Task<int> VerificaUltimoIdentificadorMovimento();

        /// <summary>
        /// Gravar movimento de conta corrente
        /// </summary>
        /// <param name="movimentacaoContaDto"></param>
        /// <returns></returns>
        Task GravarMovimentoContaCorrente(MovimentacaoContaDto movimentacaoContaDto);

        /// <summary>
        /// Consultar saldo do cliente
        /// </summary>
        /// <param name="idContaCorrente"></param>
        /// <returns></returns>
        Task<decimal> ConsultarSaldoCliente(string idContaCorrente);
    }
}
