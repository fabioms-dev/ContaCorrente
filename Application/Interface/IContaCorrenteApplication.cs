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

        /// <summary>
        /// Validar login do cliente
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns></returns>        
        Task<AutenticacaoDto> ValidarLogin(LoginRequestDto loginRequestDto);

        /// <summary>
        /// Inativar conta corrente do cliente
        /// </summary>
        /// <param name="tokenAutenticacao"></param>
        /// <param name="loginRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="ContaInvalidaException"></exception>
        /// <exception cref="UsuarioNaoAutorizadoException"></exception>
        /// <exception cref="Exception"></exception>
        Task InativarContaCorrente(string tokenAutenticacao, LoginRequestDto loginRequestDto);

        /// <summary>
        /// Movimentar conta corrente
        /// </summary>
        /// <param name="tokenAutenticacao"></param>
        /// <param name="movimentacaoContaDto"></param>
        /// <returns></returns>
        /// <exception cref="ContaInvalidaException"></exception>
        /// <exception cref="ContaInativaException"></exception>
        /// <exception cref="ValorInvalidoException"></exception>
        /// <exception cref="TipoMovimentoInvalidoException"></exception>
        /// <exception cref="Exception"></exception>
        Task MovimentarContaCorrente(string tokenAutenticacao, MovimentacaoContaDto movimentacaoContaDto);
    }
}
