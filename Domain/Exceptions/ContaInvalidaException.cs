namespace ContaCorrente.Domain.Exceptions
{
    /// <summary>
    /// Conta invalida exceção
    /// </summary>
    public class ContaInvalidaException : Exception
    {
        public ContaInvalidaException(string message) : base(message) { }
        public ContaInvalidaException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }
}
