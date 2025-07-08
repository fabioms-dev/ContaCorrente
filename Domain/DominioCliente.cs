using ContaCorrente.Domain.Dto;
using ContaCorrente.Domain.Entidade;
using ContaCorrente.Domain.Interface;
using DocumentValidator;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ContaCorrente.Domain
{
    /// <summary>
    /// Cliente domain service
    /// </summary>
    public class DominioCliente : IDominioCliente
    {
        private const int KeySize = 16;
        private const int SaltSize = 32;
        private const int Iterations = 600_000;
        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="configuration"></param>
        public DominioCliente(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Validar cpf do cliente
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public async Task<bool> ValidarClienteCpf(string cpf)
        {
            return await Task.FromResult(CpfValidation.Validate(cpf));
        }

        /// <summary>
        /// Gerar hash da senha
        /// </summary>
        /// <param name="senha"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public string GerarHashSenha(string senha, out string saltBase64)
        {
            byte[] saltBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
            
            using var pbkdf2 = new Rfc2898DeriveBytes(senha, saltBytes, 100_000, HashAlgorithmName.SHA256);
            byte[] hashBytes = pbkdf2.GetBytes(32);
            
            saltBase64 = Convert.ToBase64String(saltBytes);
            string hashBase64 = Convert.ToBase64String(hashBytes);

            return hashBase64;            
        }

        /// <summary>
        /// Gerar número da conta
        /// </summary>
        /// <returns></returns>
        public string GerarNumeroConta()
        {
            const string prefixo = "32";
            var rng = new Random();
            string parteAleatoria = rng.Next(0, 1_000_000).ToString("D6");
            return prefixo + parteAleatoria;
        }

        /// <summary>
        /// Gerar token de autenticação
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public AutenticacaoDto GerarTokenAutenticacao(string nome)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: new[] { new Claim(ClaimTypes.Name, nome) },
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new AutenticacaoDto
                    (
                       token: new JwtSecurityTokenHandler().WriteToken(token),
                       dataExpiracao: token.ValidTo
                    );
        }

        /// <summary>
        /// Validar senha do cliente
        /// </summary>
        /// <param name="clienteSenha"></param>
        /// <param name="clienteSalt"></param>
        /// <param name="loginSenha"></param>
        /// <returns></returns>
        public bool ValidarSenha(string clienteSenha, string clienteSalt, string loginSenha)
        {
            byte[] saltBytes = Convert.FromBase64String(clienteSalt);
            byte[] hashBytesArmazenado = Convert.FromBase64String(clienteSenha);

            using var pbkdf2 = new Rfc2898DeriveBytes(loginSenha, saltBytes, 100_000, HashAlgorithmName.SHA256);
            byte[] hashBytesInformado = pbkdf2.GetBytes(32);
            
            return CryptographicOperations.FixedTimeEquals(hashBytesArmazenado, hashBytesInformado);
        }
    }
}
