using Application;
using Application.Interface;
using ContaCorrente.Domain.Dto;
using ContaCorrente.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ContaCorrente.Controllers
{
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
    }
}
