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
    }
}
