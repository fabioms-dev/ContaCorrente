namespace ContaCorrente.Domain.Entidade
{
    public class Cliente
    {
        public string IdContaCorrente { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Senha { get; set; }
        public string NumeroConta { get; set; }
        public int Ativo { get; set; }
        public string Salt { get; set; }
    }
}
