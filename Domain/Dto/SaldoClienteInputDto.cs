using System.Diagnostics.CodeAnalysis;

namespace ContaCorrente.Domain.Dto
{
    /// <summary>
    /// Saldo cliente input data transfer object (DTO)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SaldoClienteInputDto
    {
        /// <summary>
        /// Identificador da conta corrente
        /// </summary>
        public string IdContaCorrente { get; set; }        
    }
}
