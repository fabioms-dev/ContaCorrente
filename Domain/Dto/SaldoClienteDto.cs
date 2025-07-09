namespace ContaCorrente.Domain.Dto
{
    /// <summary>
    /// Saldo cliente data transfer object (DTO)
    /// </summary>
    public class SaldoClienteDto
    {
        public string NumeroContaCorrente { get; set; }
        public string TitularContaCorrente { get; set; }
        public DateTime DataConsultaSaldo { get; set; }
        public decimal ValorSaldoAtual { get; set; }
    }
}
