using Application.Interface;
using ContaCorrente.Domain.Dto;
using ContaCorrente.Domain.Enum;
using ContaCorrente.Domain.Exceptions;
using ContaCorrente.Domain.Interface;

namespace ContaCorrente.Application
{
    /// <summary>
    /// Conta Corrente Application
    /// </summary>
    public class ContaCorrenteApplication : IContaCorrenteApplication
    {
        private readonly IDominioCliente _dominioCliente;
        /// <summary>
        /// Construtor da classe ContaCorrenteApplication
        /// </summary>
        /// <param name="dominioCliente"></param>
        public ContaCorrenteApplication(IDominioCliente dominioCliente)
        {
            _dominioCliente = dominioCliente;
        }

        /// <summary>
        /// Cadastrar uma conta corrente
        /// </summary>
        /// <returns></returns>
        public async Task<string> CadastrarCliente(ClienteDto clienteDto)
        {
            var isValidCpf = await _dominioCliente.ValidarClienteCpf(clienteDto.Cpf);
            
            if(!isValidCpf)
                throw new CpfInvalidoException(
                    "CPF inválido. Tipo de falha: {0}. CPF Informado: {1}",
                    TipoFalha.Invalid_Document,
                    clienteDto.Cpf);




            return "123";
        }
    }
}
