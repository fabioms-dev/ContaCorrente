using ContaCorrente.Domain.Entidade;
using System.Diagnostics.CodeAnalysis;

namespace ContaCorrente.Domain.Dto
{
    /// <summary>
    /// Saldo cliente data transfer object (DTO)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SaldoClienteDto
    {
        public string NumeroContaCorrente { get; set; }
        public string TitularContaCorrente { get; set; }
        public DateTime DataConsultaSaldo { get; set; }
        public decimal ValorSaldoAtual { get; set; }

        /// <summary>
        /// Construtor com parâmetros
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="valorSaldoAtual"></param>
        public SaldoClienteDto(Cliente cliente, decimal valorSaldoAtual)
        {
            NumeroContaCorrente = cliente.NumeroConta;
            TitularContaCorrente = cliente.Nome;
            DataConsultaSaldo = DateTime.Now;
            ValorSaldoAtual = valorSaldoAtual;
        }
    }
}
