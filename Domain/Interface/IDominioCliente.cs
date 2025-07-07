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
        /// Gerar hash da senha
        /// </summary>
        /// <param name="senha"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        string GerarHashSenha(string senha, out byte[] salt);

        /// <summary>
        /// Gerar número da conta
        /// </summary>
        /// <returns></returns>
        string GerarNumeroConta();
    }
}
