using ContaCorrente.Domain.Interface;
using DocumentValidator;

namespace ContaCorrente.Domain
{
    public class DominioCliente : IDominioCliente
    {
        public DominioCliente() { }

        /// <summary>
        /// Validar cpf do cliente
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public async Task<bool> ValidarClienteCpf(string cpf)
        {            
            return await Task.FromResult(CpfValidation.Validate(cpf));
        }
    }
}
