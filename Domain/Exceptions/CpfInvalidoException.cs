namespace ContaCorrente.Domain.Exceptions
{
    /// <summary>
    /// Exceção para CPF inválido
    /// </summary>
    public class CpfInvalidoException : Exception
    {
        public CpfInvalidoException(string message) : base(message) { }
        public CpfInvalidoException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }
}
