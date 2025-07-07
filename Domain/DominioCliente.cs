using ContaCorrente.Domain.Dto;
using ContaCorrente.Domain.Interface;
using ContaCorrente.Infrastructure.Interface;
using DocumentValidator;

namespace ContaCorrente.Domain
{
    /// <summary>
    /// Cliente domain service
    /// </summary>
    public class DominioCliente : IDominioCliente
    {
        private readonly IClienteRepository _clienteRepository;

        /// <summary>
        /// Construtor
        /// </summary>
        public DominioCliente(IClienteRepository clienteRepository) 
        {
            _clienteRepository = clienteRepository;
        }

        /// <summary>
        /// Validar cpf do cliente
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public async Task<bool> ValidarClienteCpf(string cpf)
        {            
            return await Task.FromResult(CpfValidation.Validate(cpf));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public async Task<string> CriarContaClienteNoBanco(ClienteDto cliente)
        {
            var ultimoIdentificadorBanco = await

            return "teste";
        }
    }
}
