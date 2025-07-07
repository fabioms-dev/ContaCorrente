using System.Text.Json.Serialization;

namespace ContaCorrente.Domain.Dto
{
    /// <summary>
    /// Cliente Data Transfer Object (DTO)
    /// </summary>
    public class ClienteDto
    {        
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Senha { get; set; }
        [JsonIgnore]
        public string NumeroConta { get; set; }
        [JsonIgnore]
        public int Ativo { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
    }    
}
