using System.Diagnostics.CodeAnalysis;

namespace ContaCorrente.Domain.Dto
{
    /// <summary>
    /// MovimentacaoConta Data Transfer Object (DTO)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MovimentacaoContaDto
    {
        public string idMovimento { get; set; }
        public string idContaCorrente { get; set; }
        public string DataMovimento { get; set; }
        public char? TipoMovimento { get; set; } // 'C' para crédito, 'D' para débito
        public decimal Valor { get; set; }
    }
}
