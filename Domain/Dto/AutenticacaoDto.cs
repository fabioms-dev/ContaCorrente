namespace ContaCorrente.Domain.Dto
{
    /// <summary>
    /// Autenticacao Data Transfer Object (DTO)
    /// </summary>
    public class AutenticacaoDto
    {
        /// <summary>
        /// Token de autenticação do cliente
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Data de expiração do token
        /// </summary>
        public DateTime DataExpiracao { get; set; }

        /// <summary>
        /// Construtor com parâmetros
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dataExpiracao"></param>
        public AutenticacaoDto(string token, DateTime dataExpiracao)
        {
            Token = token;
            DataExpiracao = dataExpiracao;
        }
    }
}
