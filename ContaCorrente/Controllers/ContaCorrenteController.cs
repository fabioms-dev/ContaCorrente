using Application;
using Application.Interface;
using ContaCorrente.Domain.Dto;
using ContaCorrente.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace ContaCorrente.Controllers
{
    /// <summary>
    /// Conta Corrente Controller
    /// </summary>
    [ApiController]
    [Route("")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly ILogger<ContaCorrenteController> _logger;
        private readonly IContaCorrenteApplication _contaCorrenteApplication;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="contaCorrenteApplication"></param>
        public ContaCorrenteController(ILogger<ContaCorrenteController> logger,
                                       IContaCorrenteApplication contaCorrenteApplication)
        {
            _logger = logger;
            _contaCorrenteApplication = contaCorrenteApplication;
        }

        /// <summary>
        /// Cadastrar conta corrente
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] ClienteDto clienteDto)
        {
            try
            {
                var numeroConta = await _contaCorrenteApplication.CadastrarCliente(clienteDto);
                _logger.LogInformation("GET request received for Conta Corrente");
                return Ok(numeroConta);
            }
            catch (CpfInvalidoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (CpfJaPossuiContaException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Efetuar login na conta corrente
        /// </summary>
        /// <returns> </returns>
        [HttpPost("api/login")]        
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            try
            {
                var token = await _contaCorrenteApplication.ValidarLogin(loginRequestDto);

                _logger.LogInformation("Realizado validação do login do cliente com sucesso.");
                _logger.LogInformation("Realizado a geração do token de autenticação.");

                return Ok(token);
            }
            catch (UsuarioNaoAutorizadoException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
