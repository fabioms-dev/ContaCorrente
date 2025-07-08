using Application.Interface;
using ContaCorrente.Domain.Dto;
using ContaCorrente.Domain.Entidade;
using ContaCorrente.Domain.Enum;
using ContaCorrente.Domain.Exceptions;
using ContaCorrente.Domain.Interface;
using ContaCorrente.Infrastructure.Interface;
using System.Security.Cryptography;
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

            var senha = _dominioCliente.GerarHashSenha(clienteDto.Senha, out string salt);
            var numeroConta = _dominioCliente.GerarNumeroConta();
            var cliente = new Cliente(clienteDto, salt, senha, numeroConta);

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

        /// <summary>
        /// Validar login do cliente
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="CpfInvalidoException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<AutenticacaoDto> ValidarLogin(LoginRequestDto loginRequestDto)
        {
            var cliente = await _clienteRepository.ObterClientePorCpf(loginRequestDto.Cpf) ?? throw new Exception("Cadastro não encontrado");
            var senhaHash = _dominioCliente.GerarHashSenha(loginRequestDto.Senha, out string salt);

            if (cliente.Senha.Length % 2 != 0)
                throw new ArgumentException("A hex string precisa ter um número par de caracteres.");
            
            if (!_dominioCliente.ValidarSenha(cliente.Senha, cliente.Salt, loginRequestDto.Senha))
                throw new UsuarioNaoAutorizadoException(
                    "Usuário inválido, verique suas credencias. Tipo de falha: {0}.",
                    TipoFalha.User_Unauthorized);
                                            
            return _dominioCliente.GerarTokenAutenticacao(cliente.IdContaCorrente);
        }        
    }
}
