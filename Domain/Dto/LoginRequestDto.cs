using System.Diagnostics.CodeAnalysis;

namespace ContaCorrente.Domain.Dto
{
    /// <summary>
    /// Login Request Data Transfer Object (DTO)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoginRequestDto
    {
        /// <summary>
        /// Numero da conta do cliente
        /// </summary>
        public string NumeroConta { get; set; }

        /// <summary>
        /// Cpf do cliente
        /// </summary>
        public string Cpf { get; set; }

        /// <summary>
        /// Senha do cliente
        /// </summary>
        public string Senha { get; set; }
    }
}
