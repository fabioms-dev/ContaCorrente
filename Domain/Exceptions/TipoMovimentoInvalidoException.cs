namespace ContaCorrente.Domain.Exceptions
{
    /// <summary>
    /// Tipo de movimento exceção
    /// </summary>
    public class TipoMovimentoInvalidoException : Exception
    {
        public TipoMovimentoInvalidoException(string message) : base(message) { }
        public TipoMovimentoInvalidoException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }
}
