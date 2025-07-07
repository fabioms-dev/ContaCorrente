using ContaCorrente.Domain.Dto;
using ContaCorrente.Domain.Interface;
using DocumentValidator;
using System.Security.Cryptography;
using System.Text;

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

        /// <summary>
        /// Construtor
        /// </summary>
        public DominioCliente()
        {
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
        public string GerarHashSenha(string senha, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(senha),
                salt,
                Iterations,
                HashAlgorithm,
                KeySize
            );

            return Convert.ToHexString(hash);
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
    }
}
