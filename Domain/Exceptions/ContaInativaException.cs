namespace ContaCorrente.Domain.Exceptions
{
    /// <summary>
    /// Conta inativa exceção
    /// </summary>
    public class ContaInativaException : Exception
    {
        public ContaInativaException(string message) : base(message) { }
        public ContaInativaException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }
}
