namespace ContaCorrente.Domain.Exceptions
{
    /// <summary>
    /// Valor inválido exceção
    /// </summary>
    public class ValorInvalidoException : Exception
    {
        public ValorInvalidoException(string message) : base(message) { }
        public ValorInvalidoException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }
}
