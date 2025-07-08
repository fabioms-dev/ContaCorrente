namespace ContaCorrente.Domain.Exceptions
{
    /// <summary>
    /// Usuario não autorizado exceção
    /// </summary>
    public class UsuarioNaoAutorizadoException : Exception 
    {
        public UsuarioNaoAutorizadoException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }
}
