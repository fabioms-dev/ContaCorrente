using ContaCorrente.Domain.Dto;

namespace ContaCorrente.Domain.Interface
{
    public interface IDominioCliente
    {
        /// <summary>
        /// Validar cpf do cliente
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        Task<bool> ValidarClienteCpf(string cpf);

        /// <summary>
        /// Criar conta do cliente no banco de dados
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        Task<string> CriarContaClienteNoBanco(ClienteDto cliente);
    }
}
