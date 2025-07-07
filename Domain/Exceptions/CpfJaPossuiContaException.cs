namespace ContaCorrente.Domain.Exceptions
{
    /// <summary>
    /// Exceção para CPF ja possui conta
    /// </summary>
    public class CpfJaPossuiContaException : Exception
    {
        public CpfJaPossuiContaException(string message) : base(message) { }        
    }
}
