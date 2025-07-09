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
        private readonly IMovimentoRepository _movimentoRepository;

        /// <summary>
        /// Construtor da classe ContaCorrenteApplication
        /// </summary>
        /// <param name="dominioCliente"></param>
        /// <param name="clienteRepository"></param>
        /// <param name="movimentoRepository"></param>
        public ContaCorrenteApplication(IDominioCliente dominioCliente,
                                        IClienteRepository clienteRepository,
                                        IMovimentoRepository movimentoRepository)
        {
            _dominioCliente = dominioCliente;
            _clienteRepository = clienteRepository;
            _movimentoRepository = movimentoRepository;
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

            if (!_dominioCliente.ValidarSenha(cliente.Senha, cliente.Salt, loginRequestDto.Senha))
                throw new UsuarioNaoAutorizadoException(
                    "Usuário inválido, verique suas credencias. Tipo de falha: {0}.",
                    TipoFalha.User_Unauthorized);

            return _dominioCliente.GerarTokenAutenticacao(cliente.IdContaCorrente);
        }

        /// <summary>
        /// Inativar conta corrente do cliente
        /// </summary>
        /// <param name="tokenAutenticacao"></param>
        /// <param name="loginRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="ContaInvalidaException"></exception>
        /// <exception cref="UsuarioNaoAutorizadoException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task InativarContaCorrente(string tokenAutenticacao, LoginRequestDto loginRequestDto)
        {
            var cliente = await _clienteRepository.ObterClientePorCpf(loginRequestDto.Cpf) ?? throw new ContaInvalidaException("Conta invalida. Tipo de falha: {0}.", TipoFalha.Invalid_Account);

            if (!_dominioCliente.ValidarSenha(cliente.Senha, cliente.Salt, loginRequestDto.Senha) && !_dominioCliente.ValidarDataExpiracaoToken(tokenAutenticacao))
                throw new UsuarioNaoAutorizadoException(
                    "Usuário inválido, verique suas credencias. Tipo de falha: {0}.",
                    TipoFalha.User_Unauthorized);
            try
            {
                await _clienteRepository.InativarContaCliente(cliente.IdContaCorrente);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar dados do cliente: " + ex.Message);
            }
        }

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
        public async Task MovimentarContaCorrente(string tokenAutenticacao, MovimentacaoContaDto movimentacaoContaDto)
        {
            var cliente = await _clienteRepository.ObterClientePorCpf(movimentacaoContaDto.idContaCorrente) ?? throw new ContaInvalidaException("Conta invalida. Tipo de falha: {0}.", TipoFalha.Invalid_Account);

            if (!_dominioCliente.ValidarDataExpiracaoToken(tokenAutenticacao))
                throw new UsuarioNaoAutorizadoException(
                    "Token expirado. Tipo de falha: {0}.",
                    TipoFalha.User_Unauthorized);

            if (cliente.Ativo == 0)
                throw new ContaInativaException("Conta inativa. Tipo de falha: {0}.", TipoFalha.Inactive_Account);

            if (movimentacaoContaDto.Valor < 0)
                throw new ValorInvalidoException("Valor inválido para movimentação. Tipo de falha: {0}.", TipoFalha.Invalid_Account);

            if ((!movimentacaoContaDto.TipoMovimento.Equals(TipoMovimento.Credito)) && (!movimentacaoContaDto.TipoMovimento.Equals(TipoMovimento.Debito)))
                throw new TipoMovimentoInvalidoException("Tipo de movimento invalido. Tipo de falha: {0}.", TipoFalha.Invalid_Type);

            movimentacaoContaDto.idContaCorrente = (await _movimentoRepository.VerificaUltimoIdentificadorMovimento() + 1).ToString();

            try
            {
                await _movimentoRepository.GravarMovimentoContaCorrente(movimentacaoContaDto);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar movimento da conta corrente: " + ex.Message);
            }
        }
    }
}
