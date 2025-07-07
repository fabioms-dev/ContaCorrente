using ContaCorrente.Domain.Dto;

namespace Application.Interface
{
    /// <summary>
    /// Conta Corrente Application Interface
    /// </summary>
    public interface IContaCorrenteApplication
    {
        /// <summary>
        /// Cadastrar conta corrente
        /// </summary>
        /// <returns>Task<string></returns>
        /// <param name="clienteDto"></param>
        Task<string> CadastrarCliente(ClienteDto clienteDto);
    }
}
