using ContaCorrente.Domain.Dto;

namespace ContaCorrente.Domain.Entidade
{
    /// <summary>
    /// Cliente entidade
    /// </summary>
    public class Cliente
    {
        public string IdContaCorrente { get; set; }
        public string Nome { get; set; }        
        public string Senha { get; set; }
        public string NumeroConta { get; set; }
        public int Ativo { get; set; }
        public string Salt { get; set; }

        /// <summary>
        /// Construtor com parâmetros para criar um novo cliente
        /// </summary>
        /// <param name="clienteDto"></param>        
        /// <param name="salt"></param>
        /// <param name="senha"></param>
        /// <param name="numeroConta"></param>
        public Cliente(ClienteDto clienteDto, string salt, string senha, string numeroConta)
        {
            IdContaCorrente = clienteDto.Cpf;
            Nome = clienteDto.Nome;            
            Senha = senha;
            NumeroConta = numeroConta;
            Ativo = 1;
            Salt = salt;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        public Cliente() { }

    }
}
