using ContaCorrente.Application;
using ContaCorrente.Domain.Dto;
using ContaCorrente.Domain.Entidade;
using ContaCorrente.Domain.Enum;
using ContaCorrente.Domain.Exceptions;
using ContaCorrente.Domain.Interface;
using ContaCorrente.Infrastructure.Interface;
using Moq;
using NPOI.SS.Formula.Functions;
using Xunit;

namespace ContaCorrente.Testes
{
    /// <summary>
    /// Conta Corrente Application Tests
    /// </summary>
    public class ContaCorrenteApplicationTests
    {
        private readonly Mock<IDominioCliente> _dominioClienteMock;
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<IMovimentoRepository> _movimentoRepositoryMock;
        private readonly ContaCorrenteApplication _app;

        /// <summary>
        /// Construtor da classe ContaCorrenteApplicationTests
        /// </summary>
        public ContaCorrenteApplicationTests()
        {
            _dominioClienteMock = new Mock<IDominioCliente>();
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _movimentoRepositoryMock = new Mock<IMovimentoRepository>();

            _app = new ContaCorrenteApplication(
                _dominioClienteMock.Object,
                _clienteRepositoryMock.Object,
                _movimentoRepositoryMock.Object);
        }

        /// <summary>
        /// Cadastrar cliente deve retornar numero da conta quando os dados forem válidos
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CadastrarCliente_DeveRetornarNumeroConta_QuandoDadosValidos()
        {            
            var clienteDto = new ClienteDto { Cpf = "12345678900", Senha = "senha123" };
            var senhaHash = "hash";
            var salt = "salt";
            var numeroConta = "000123";

            _dominioClienteMock.Setup(x => x.ValidarClienteCpf(clienteDto.Cpf)).ReturnsAsync(true);
            _clienteRepositoryMock.Setup(x => x.VerificaSeCpfJaEstaCadastrado(clienteDto.Cpf)).ReturnsAsync(0);
            _dominioClienteMock.Setup(x => x.GerarHashSenha(clienteDto.Senha, out salt)).Returns(senhaHash);
            _dominioClienteMock.Setup(x => x.GerarNumeroConta()).Returns(numeroConta);
            _clienteRepositoryMock.Setup(x => x.GravarDadosCliente(It.IsAny<Cliente>())).Returns(Task.CompletedTask);
            
            var resultado = await _app.CadastrarCliente(clienteDto);
            
            Assert.Equal(numeroConta, resultado);
        }

        /// <summary>
        /// validar login deve retornar AutenticacaoDto quando o login for válido
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidarLogin_DeveRetornarAutenticacaoDto_QuandoLoginValido()
        {            
            var cpf = "12345678900";
            var senha = "senha123";
            var cliente = new Cliente
            {
                IdContaCorrente = cpf,
                Senha = "senhaHash",
                Salt = "salt",                
            };
            var loginDto = new LoginRequestDto { Cpf = cpf, Senha = senha };
            var autenticacaoDto = new AutenticacaoDto ("token123", DateTime.Now.AddHours(1));

            _clienteRepositoryMock.Setup(x => x.ObterClientePorCpf(cpf)).ReturnsAsync(cliente);
            _dominioClienteMock.Setup(x => x.ValidarSenha(cliente.Senha, cliente.Salt, senha)).Returns(true);
            _dominioClienteMock.Setup(x => x.GerarTokenAutenticacao(cliente.IdContaCorrente)).Returns(autenticacaoDto);
            
            var resultado = await _app.ValidarLogin(loginDto);
            
            Assert.NotNull(resultado);
            Assert.Equal("token123", resultado.Token);
        }

        /// <summary>
        /// Validar login deve lançar CpfInvalidoException quando o CPF for inválido
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidarLogin_DeveLancarUsuarioNaoAutorizadoException_QuandoSenhaInvalida()
        {            
            var cpf = "12345678900";
            var senha = "senhaErrada";
            var cliente = new Cliente { IdContaCorrente = cpf, Senha = "senhaHash", Salt = "salt" };
            var loginDto = new LoginRequestDto { Cpf = cpf, Senha = senha };

            _clienteRepositoryMock.Setup(x => x.ObterClientePorCpf(cpf)).ReturnsAsync(cliente);
            _dominioClienteMock.Setup(x => x.ValidarSenha(cliente.Senha, cliente.Salt, senha)).Returns(false);
            
            await Assert.ThrowsAsync<UsuarioNaoAutorizadoException>(() => _app.ValidarLogin(loginDto));
        }

        /// <summary>
        /// Inativar conta corrente deve inativar a conta quando a autenticação for válida
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task InativarContaCorrente_DeveInativarConta_QuandoAutenticacaoValida()
        {            
            var cpf = "12345678900";
            var senha = "senha123";
            var token = "tokenValido";
            var cliente = new Cliente
            {                
                Senha = "senhaHash",
                Salt = "salt",
                IdContaCorrente = "000123"
            };
            var loginDto = new LoginRequestDto { Cpf = cpf, Senha = senha };

            _clienteRepositoryMock.Setup(x => x.ObterClientePorCpf(cpf)).ReturnsAsync(cliente);
            _dominioClienteMock.Setup(x => x.ValidarSenha(cliente.Senha, cliente.Salt, senha)).Returns(true);
            _dominioClienteMock.Setup(x => x.ValidarDataExpiracaoToken(token)).Returns(true);
            _clienteRepositoryMock.Setup(x => x.InativarContaCliente(cliente.IdContaCorrente)).Returns(Task.CompletedTask);
            
            await _app.InativarContaCorrente(token, loginDto);
            
            _clienteRepositoryMock.Verify(x => x.InativarContaCliente(cliente.IdContaCorrente), Times.Once);
        }

        /// <summary>
        /// Inalidar conta corrente deve lançar ContaInvalidaException quando o cliente não for encontrado
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task InativarContaCorrente_DeveLancarContaInvalidaException_QuandoClienteNaoEncontrado()
        {
            var loginDto = new LoginRequestDto { Cpf = "00000000000", Senha = "senha" };
            _clienteRepositoryMock.Setup(x => x.ObterClientePorCpf(loginDto.Cpf)).ReturnsAsync((Cliente)null);
            
            await Assert.ThrowsAsync<ContaInvalidaException>(() => _app.InativarContaCorrente("token", loginDto));
        }

        /// <summary>
        /// Inativar conta corrente deve lançar UsuarioNaoAutorizadoException quando a senha ou token forem inválidos
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task InativarContaCorrente_DeveLancarUsuarioNaoAutorizadoException_QuandoSenhaEtokenInvalidos()
        {            
            var cliente = new Cliente { Senha = "hash", Salt = "salt", IdContaCorrente = "000123" };
            var loginDto = new LoginRequestDto { Cpf = "12345678900", Senha = "senhaErrada" };

            _clienteRepositoryMock.Setup(x => x.ObterClientePorCpf(loginDto.Cpf)).ReturnsAsync(cliente);
            _dominioClienteMock.Setup(x => x.ValidarSenha(cliente.Senha, cliente.Salt, loginDto.Senha)).Returns(false);
            _dominioClienteMock.Setup(x => x.ValidarDataExpiracaoToken("token")).Returns(false);
            
            await Assert.ThrowsAsync<UsuarioNaoAutorizadoException>(() => _app.InativarContaCorrente("token", loginDto));
        }

        /// <summary>
        /// Inativar conta corrente deve lançar Exception quando ocorrer falha ao inativar a conta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task InativarContaCorrente_DeveLancarException_QuandoFalhaAoInativarConta()
        {            
            var cpf = "12345678900";
            var senha = "senha123";
            var token = "tokenValido";
            var cliente = new Cliente
            {                
                Senha = "senhaHash",
                Salt = "salt",
                IdContaCorrente = "000123"
            };
            var loginDto = new LoginRequestDto { Cpf = cpf, Senha = senha };

            _clienteRepositoryMock.Setup(x => x.ObterClientePorCpf(cpf)).ReturnsAsync(cliente);
            _dominioClienteMock.Setup(x => x.ValidarSenha(cliente.Senha, cliente.Salt, senha)).Returns(true);
            _dominioClienteMock.Setup(x => x.ValidarDataExpiracaoToken(token)).Returns(true);
            
            _clienteRepositoryMock
                .Setup(x => x.InativarContaCliente(cliente.IdContaCorrente))
                .ThrowsAsync(new Exception("Erro simulado"));
            
            var ex = await Assert.ThrowsAsync<Exception>(() => _app.InativarContaCorrente(token, loginDto));
            Assert.Contains("Erro ao atualizar dados da conta do cliente", ex.Message);
        }

        /// <summary>
        /// Movimentar conta corrente deve gravar movimento quando os dados forem válidos
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task MovimentarContaCorrente_DeveGravarMovimento_QuandoDadosValidos()
        {            
            var token = "tokenValido";
            var cliente = new Cliente { Ativo = 1 };
            var movimentacaoDto = new MovimentacaoContaDto
            {
                idContaCorrente = "123",
                Valor = 100,
                TipoMovimento = 'C'
            };

            _clienteRepositoryMock.Setup(x => x.ObterClientePorCpf("123")).ReturnsAsync(cliente);
            _dominioClienteMock.Setup(x => x.ValidarDataExpiracaoToken(token)).Returns(true);
            _movimentoRepositoryMock.Setup(x => x.VerificaUltimoIdentificadorMovimento()).ReturnsAsync(10);
            _movimentoRepositoryMock.Setup(x => x.GravarMovimentoContaCorrente(It.IsAny<MovimentacaoContaDto>())).Returns(Task.CompletedTask);
            
            await _app.MovimentarContaCorrente(token, movimentacaoDto);
            
            _movimentoRepositoryMock.Verify(x => x.GravarMovimentoContaCorrente(It.IsAny<MovimentacaoContaDto>()), Times.Once);
        }

        /// <summary>
        /// Movimentar conta corrente deve lançar ContaInvalidaException quando o cliente não for encontrado
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task MovimentarContaCorrente_DeveLancarTipoMovimentoInvalidoException_QuandoTipoInvalido()
        {            
            var cliente = new Cliente { Ativo = 1 };
            var dto = new MovimentacaoContaDto
            {
                idContaCorrente = "123",
                Valor = 100,
                TipoMovimento = 'X'
            };

            _clienteRepositoryMock.Setup(x => x.ObterClientePorCpf("123")).ReturnsAsync(cliente);
            _dominioClienteMock.Setup(x => x.ValidarDataExpiracaoToken("token")).Returns(true);
            
            await Assert.ThrowsAsync<TipoMovimentoInvalidoException>(() => _app.MovimentarContaCorrente("token", dto));
        }

        /// <summary>
        /// Consultar saldo cliente deve retornar SaldoClienteDto quando os dados forem válidos
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ConsultarSaldoCliente_DeveRetornarSaldoClienteDto_QuandoDadosValidos()
        {            
            var token = "tokenValido";
            var idConta = "123";
            var cliente = new Cliente { IdContaCorrente = idConta, Ativo = 1 };
            var saldoEsperado = 500.00m;

            _clienteRepositoryMock.Setup(x => x.ObterClientePorCpf(idConta)).ReturnsAsync(cliente);
            _dominioClienteMock.Setup(x => x.ValidarDataExpiracaoToken(token)).Returns(true);
            _movimentoRepositoryMock.Setup(x => x.ConsultarSaldoCliente(idConta)).ReturnsAsync(saldoEsperado);
            
            var resultado = await _app.ConsultarSaldoCliente(token, idConta);
            
            Assert.NotNull(resultado);
            Assert.Equal(saldoEsperado, resultado.ValorSaldoAtual);            
        }

        /// <summary>
        /// Consultar saldo cliente deve lançar ContaInvalidaException quando o cliente não for encontrado
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ConsultarSaldoCliente_DeveLancarContaInvalidaException_QuandoClienteNaoEncontrado()
        {            
            var idConta = "999";
            _clienteRepositoryMock.Setup(x => x.ObterClientePorCpf(idConta)).ReturnsAsync((Cliente)null);
            
            await Assert.ThrowsAsync<ContaInvalidaException>(() => _app.ConsultarSaldoCliente("token", idConta));
        }

        /// <summary>
        /// Consultar saldo cliente deve lançar UsuarioNaoAutorizadoException quando o token estiver expirado
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ConsultarSaldoCliente_DeveLancarUsuarioNaoAutorizadoException_QuandoTokenExpirado()
        {            
            var idConta = "123";
            var cliente = new Cliente { IdContaCorrente = idConta, Ativo = 1 };

            _clienteRepositoryMock.Setup(x => x.ObterClientePorCpf(idConta)).ReturnsAsync(cliente);
            _dominioClienteMock.Setup(x => x.ValidarDataExpiracaoToken("token")).Returns(false);
            
            await Assert.ThrowsAsync<UsuarioNaoAutorizadoException>(() => _app.ConsultarSaldoCliente("token", idConta));
        }

        /// <summary>
        /// Consultar saldo cliente deve lançar ContaInativaException quando a conta estiver inativa
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ConsultarSaldoCliente_DeveLancarContaInativaException_QuandoContaInativa()
        {            
            var idConta = "123";
            var cliente = new Cliente { IdContaCorrente = idConta, Ativo = 0 };

            _clienteRepositoryMock.Setup(x => x.ObterClientePorCpf(idConta)).ReturnsAsync(cliente);
            _dominioClienteMock.Setup(x => x.ValidarDataExpiracaoToken("token")).Returns(true);
            
            await Assert.ThrowsAsync<ContaInativaException>(() => _app.ConsultarSaldoCliente("token", idConta));
        }
    }
}
