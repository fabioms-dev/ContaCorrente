using Application.Interface;
using ContaCorrente.Domain.Dto;
using ContaCorrente.Domain.Entidade;
using ContaCorrente.Domain.Enum;
using ContaCorrente.Domain.Exceptions;
using ContaCorrente.Domain.Interface;
using ContaCorrente.Infrastructure.Interface;
using System.Text;

namespace ContaCorrente.Application
{
    /// <summary>
    /// Conta Corrente Application
    /// </summary>
    public class ContaCorrenteApplication : IContaCorrenteApplication
    {
        private readonly IDominioCliente _dominioCliente;
        private readonly IClienteRepository _clienteRepository;

        /// <summary>
        /// Construtor da classe ContaCorrenteApplication
        /// </summary>
        /// <param name="dominioCliente"></param>
        /// <param name="clienteRepository"></param>
        public ContaCorrenteApplication(IDominioCliente dominioCliente,
                                        IClienteRepository clienteRepository)
        {
            _dominioCliente = dominioCliente;
            _clienteRepository = clienteRepository;
        }

        /// <summary>
        /// Cadastrar uma conta corrente
        /// </summary>
        /// <returns></returns>
        public async Task<string> CadastrarCliente(ClienteDto clienteDto)
        {
            var isValidCpf = await _dominioCliente.ValidarClienteCpf(clienteDto.Cpf);

            if (!isValidCpf)
                throw new CpfInvalidoException(
                    "CPF inválido. Tipo de falha: {0}. CPF Informado: {1}",
                    TipoFalha.Invalid_Document,
                    clienteDto.Cpf);

            if (await _clienteRepository.VerificaSeCpfJaEstaCadastrado(clienteDto.Cpf) == 1)
                throw new CpfJaPossuiContaException("CPF já possui conta cadastrada");

            var senha = _dominioCliente.GerarHashSenha(clienteDto.Senha, out byte[] salt);
            var numeroConta = _dominioCliente.GerarNumeroConta();
            var cliente = new Cliente(clienteDto, Encoding.UTF8.GetString(salt), senha, numeroConta);

            try
            {
                await _clienteRepository.GravarDadosCliente(cliente);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar dados do cliente: " + ex.Message);
            }

            return numeroConta;
        }
    }
}
