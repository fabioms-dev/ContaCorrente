using ContaCorrente.Domain.Dto;

namespace ContaCorrente.Domain.Interface
{
    /// <summary>
    /// Dominio Cliente Interface
    /// </summary>
    public interface IDominioCliente
    {
        /// <summary>
        /// Validar cpf do cliente
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        Task<bool> ValidarClienteCpf(string cpf);

        /// <summary>
        /// Gerar hash da senha
        /// </summary>
        /// <param name="senha"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        string GerarHashSenha(string senha, out string saltBase64);

        /// <summary>
        /// Gerar número da conta
        /// </summary>
        /// <returns></returns>
        string GerarNumeroConta();

        /// <summary>
        /// Gerar token de autenticação
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        AutenticacaoDto GerarTokenAutenticacao(string nome);

        /// <summary>
        /// Validar senha do cliente
        /// </summary>
        /// <param name="clienteSenha"></param>
        /// <param name="clienteSalt"></param>
        /// <param name="loginSenha"></param>
        /// <returns></returns>
        bool ValidarSenha(string clienteSenha, string clienteSalt, string loginSenha);
    }
}
