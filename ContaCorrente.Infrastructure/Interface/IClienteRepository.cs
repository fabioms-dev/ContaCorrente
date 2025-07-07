using ContaCorrente.Domain.Dto;
using ContaCorrente.Domain.Entidade;

namespace ContaCorrente.Infrastructure.Interface
{
    public interface IClienteRepository
    {
        /// <summary>
        /// Obter o ultimo identificador do banco de dados
        /// </summary>
        /// <returns></returns>
        /// <param name="idcontacorrente"></param>
        Task<int> VerificaSeCpfJaEstaCadastrado(string idcontacorrente);

        /// <summary>
        /// Gravar dados do cliente no banco de dados
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        Task GravarDadosCliente(Cliente cliente);
    }
}
